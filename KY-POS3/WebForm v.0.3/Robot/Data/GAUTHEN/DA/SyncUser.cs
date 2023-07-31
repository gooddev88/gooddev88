using Newtonsoft.Json;
using Robot.Data.DataAccess;
using Robot.Data.GAUTHEN.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.GAUTHEN.DA {
    public class SyncUser {
       
        public static List<UserSet> CreateUserSet(string username="") {
            //ถ้าส่งค่าว่างให้ Sync ทั้งหมด
            List<UserSet> docs = new List<UserSet>();
            
            try {
                using (GAEntities db = new GAEntities()) {
                    var user = db.UserInfo.Where(o =>
                                                    (o.Username == username || username == "")
                                                    ).ToList();
                    foreach (var u in user) {
                        UserSet doc = new UserSet();
                        doc.AppID = "KYPOS";
                        doc.UserInfos = new BL.UserInfo();

                        doc.UserInfos.Username = u.Username;
                        doc.UserInfos.Password = u.Password;
                        doc.UserInfos.Name = u.FullName;
                        doc.UserInfos.IsLock = !u.IsActive;
                        doc.UserInfos.IsActive = u.IsActive;

                        doc.UserInRCom = new List<UserInRcom>();
                               var rcoms = db.UserInRCom.Where(o => (o.UserName == doc.UserInfos.Username)).ToList();
                        foreach (var r in rcoms) {
                            BL.UserInRcom rn = new BL.UserInRcom();
                            rn.AppId = "KYPOS";
                            rn.RcomId = r.RComID;
                            rn.Username = r.UserName;
                            rn.IsLock = false;
                            rn.IsActive = true;
                            doc.UserInRCom.Add(rn);
                        }
                        docs.Add(doc);
                    }
                }
          
            } catch (Exception) {

                throw;
            }
          
            return docs;

        }
        public static ADBInfo GetIsProductionDB() {
            ADBInfo r = new ADBInfo();
            try {
                using (GAEntities db = new GAEntities()) {
                   r= db.ADBInfo.FirstOrDefault();
                }
            } catch (Exception) {

                throw;
            }
            return r;
         
        }

       async public static Task<I_BasicResult> UpdateUserToGAuthen(string username = "") {
            //ถ้าส่งค่าว่างให้ Sync ทั้งหมด
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                bool isProDb = false;
                isProDb= Convert.ToBoolean( GetIsProductionDB().IsProductionDB);
                if (!isProDb) {
                    return result;
                }
                   var docs = CreateUserSet(username);
                var link = LinkService.GetLinkByLinkName("gauthen_url"); 
                string method = $"/User/UpdateUser";
                var url = link + method;  
                var client = new HttpClient(); 
                string sContentType = "application/json"; 
                var json_in = JsonConvert.SerializeObject(docs);
                var content = new StringContent(json_in, Encoding.UTF8, sContentType);
                var data_back = await client.PostAsync(url, content);
                //data_back.EnsureSuccessStatusCode();
             
                string json_back = await data_back.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<I_BasicResult>(json_back);  
            } catch (Exception ex) {
                var aa = ex.Message;
                throw;
            }
            return result; 
        }
    }
}