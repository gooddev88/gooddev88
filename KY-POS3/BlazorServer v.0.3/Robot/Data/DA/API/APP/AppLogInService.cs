using Robot.Data.DA.API.Model;
using Robot.Data.GADB.TT;
using Robot.Helper.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.API.APP
{
    public class AppLogInService
    {

        public class RequestLogin
        {
            public string AppID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string ForwardUrl { get; set; }
            public string RComID { get; set; }

        }

        public class ILogInResult
        {
            public UserInfoM UserInfo { get; set; }
            public List<UserInRCom> UserInRCom { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }




        #region  Login transction

        public static ILogInResult Login(RequestLogin data)
        {
            string username = data.Username;
            string password = data.Password;

            ILogInResult result = NewLogInResultSet();
            result.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {

                string encrypt_password = Hash.hashPassword("MD5", password);
                using (GAEntities db = new GAEntities())
                {


                    UserInfo user;
                    user = db.UserInfo.Where(o => o.Username.ToLower() == username.ToLower() && o.IsProgramUser).FirstOrDefault();


                    if (user == null)
                    {
                        result.OutputAction.Result = "fail";
                        result.OutputAction.Message1 = "There is no user in system";
                        return result;
                    }
                    if (user.IsActive == false)
                    {
                        result.OutputAction.Result = "fail";
                        result.OutputAction.Message1 = "This user was blocked";
                        return result;
                    }

                    if (user.Password != encrypt_password)
                    {
                        result.OutputAction.Result = "fail";
                        result.OutputAction.Message1 = "Incorrect username or password";
                        return result;
                    }


                    result.UserInRCom = db.UserInRCom.Where(o => o.UserName == user.Username).ToList();

                    if (result.UserInRCom.Count == 0)
                    {
                        result.OutputAction.Result = "fail";
                        result.OutputAction.Message1 = "no company register";
                        return result;
                    }

                    result.UserInfo.Username = user.Username;
                    result.UserInfo.Password = user.Password;
                    result.UserInfo.FullName = user.FullName;
                    result.UserInfo.Tel = user.Tel;
                    result.UserInfo.Email = user.Email;
                    result.UserInfo.RComID = "";
                    result.UserInfo.ComID = "";
                    result.UserInfo.IsActive = true;

                }
            }
            catch (Exception ex)
            {
                result.OutputAction.Result = "fail";

                if (ex.InnerException != null)
                {
                    result.OutputAction.Message1 = ex.InnerException.Message.ToString();
                }
                else
                {
                    result.OutputAction.Message1 = ex.Message;
                }

            }
            return result;
        }





        #endregion

        #region New transaction
        public static ILogInResult NewLogInResultSet()
        {

            ILogInResult r = new ILogInResult();
            r.UserInfo = new UserInfoM();
            r.UserInRCom = new List<UserInRCom>();
            r.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return r;

        }

        #endregion



        #region Mac service
        public static I_BasicResult RegisterMac(MacRegister data)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var query = db.MacRegister.Where(o => o.IsActive && o.RComID == data.RComID && o.ComID == data.ComID && o.IsUse && o.DeviceID == data.DeviceID).FirstOrDefault();
                    if (query != null)
                    {
                        r.Result = "fail";
                        r.Message1 = "Mac lock by another device.";
                        return r;
                    }
                    db.MacRegister.Add(data);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }
            }

            return r;

        }
        public static I_BasicResult RemoveRegisterMac(string rcom, string com, string deviceId)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var query = db.MacRegister.Where(o => o.DeviceID == deviceId && o.ComID == com && o.RComID == rcom && o.IsUse).ToList();
                    foreach (var q in query)
                    {
                        q.IsUse = false;
                        db.Update(q);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }
            }

            return r;

        }
        public static List<string> ListMacUseable(string rcom, string com)
        {
            List<string> macList = new List<string>();
            try
            {
                using (GAEntities db = new GAEntities())
                {

                    macList.Add("A");
                    macList.Add("B");
                    macList.Add("C");
                    macList.Add("D");
                    macList.Add("E");
                    macList.Add("F");
                    var query = db.MacRegister.Where(o => o.IsActive && o.RComID == rcom && o.ComID == com && o.IsUse).Select(o => o.MacID).Distinct().ToList();

                    foreach (var q in query)
                    {
                        macList.Remove(q);
                    }


                }
            }
            catch (Exception ex)
            {

            }

            return macList;

        }
        #endregion
    }
}
