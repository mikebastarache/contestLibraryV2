using System;
using System.Linq;
using MMContest.Dal;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MMContest
{
    public class FacebookLogic : ContestLogic
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        
        public string EnterContest(string fbuid, int contestId)
        {
            string result = "errorOccurred";

            if (fbuid != null)
            {
                if (!DoesFbuidExist(fbuid))
                    result = "userDoesNotExistInCRM";
                else if (!HasUserEnteredThisContest(fbuid, contestId))
                    result = "userHasNotEnteredThisContest";
                else if (HasUserEnteredContestToday(fbuid, contestId))
                    result = "userHasAlreadyEnteredToday";
                else
                    result = "userIsReadyToEnterTheContest";
            }

            return result;
        }

        public string EnterContest(string fbuid, int contestId, string email)
        {
            string result = "errorOccurred";

            if (email != null && fbuid != null)
            {
                var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);

                if (user != null)
                {
                    if (string.IsNullOrEmpty(user.Fbuid))
                    {
                        user.Fbuid = fbuid;

                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.Save();
                    }

                    if (!DoesFbuidExist(fbuid))
                        result = "userDoesNotExistInCRM";
                    else if (!HasUserEnteredThisContest(fbuid, contestId))
                        result = "userHasNotEnteredThisContest";
                    else if (HasUserEnteredContestToday(fbuid, contestId))
                        result = "userHasAlreadyEnteredToday";
                    else
                        result = "userIsReadyToEnterTheContest";
                }
                else
                {
                    result = "userDoesNotExistInCRM";
                }
            }

            return result;
        }

        public int GetUserId(string fbuid)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Fbuid == fbuid);

            if (user != null)
                return user.UserId;
            return -1;
        }

        public bool DoesFbuidExist(string fbuid)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Fbuid == fbuid);

            return (user != null);
        }

        public bool HasUserEnteredThisContest(string fbuid, int contestId)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Fbuid == fbuid);

            var counter = user.UserContestRegistrations.Count(ucr => ucr.ContestId == contestId);

            return (counter > 0);
        }

        public bool HasUserEnteredContestToday(string fbuid, int contestId)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Fbuid == fbuid);

            var counter = user.Ballots.Count(ballot => Convert.ToDateTime(ballot.DateCreated).Date == DateTime.Now.Date && ballot.ContestId == contestId);

            return (counter > 0);
        }

        public Models.Signed_Request DecodeSignedRequest(string signed_request)
        {
            string payload = signed_request.Split('.')[1];
            var encoding = new UTF8Encoding();
            var decodedJson = payload.Replace("=", string.Empty).Replace('-', '+').Replace('_', '/');
            var base64JsonArray = Convert.FromBase64String(decodedJson.PadRight(decodedJson.Length + (4 - decodedJson.Length % 4) % 4, '='));
            var json = encoding.GetString(base64JsonArray);
            var o = JObject.Parse(json);

            //CREATE MODEL TO RETURN THE DATA
            Models.Signed_Request userSignedRequest = new Models.Signed_Request();

            //CREATE JSON DATA OBJECT
            string userJsonData = Convert.ToString(o.SelectToken("user")).Replace("\"", "");
            JObject userJsonObject = JObject.Parse(userJsonData);

            string pageJsonData = Convert.ToString(o.SelectToken("page")).Replace("\"", "");
            JObject pageJsonObject = JObject.Parse(userJsonData);

            userSignedRequest.code = Convert.ToString(o.SelectToken("code")).Replace("\"", "");
            userSignedRequest.algorithm = Convert.ToString(o.SelectToken("algorithm")).Replace("\"", "");
            userSignedRequest.issued_at = Convert.ToString(o.SelectToken("issued_at")).Replace("\"", "");
            userSignedRequest.fbuid = Convert.ToString(o.SelectToken("user_id")).Replace("\"", "");
            userSignedRequest.user = userJsonObject;
            userSignedRequest.country = Convert.ToString(o.SelectToken("user.country")).Replace("\"", "");
            userSignedRequest.locale = Convert.ToString(o.SelectToken("user.locale")).Replace("\"", "");
            userSignedRequest.oauth_token = Convert.ToString(o.SelectToken("oauth_token")).Replace("\"", "");
            userSignedRequest.expires = Convert.ToString(o.SelectToken("expires")).Replace("\"", "");
            userSignedRequest.app_data = Convert.ToString(o.SelectToken("app_data")).Replace("\"", "");
            userSignedRequest.page = pageJsonObject;
            userSignedRequest.pageId = Convert.ToString(o.SelectToken("page.id")).Replace("\"", "");
            userSignedRequest.pageLiked = Convert.ToString(o.SelectToken("page.liked")).Replace("\"", "");

            return userSignedRequest;
        }
    }
}
