using Dapper;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.MainDB.TT;
using System.Data.SqlClient;

namespace RobotAPI.Data.DA.User
{
    public class UserService
    {
        public static List<UserInfo> ListUser(string search)
        {
            List<UserInfo> user = new List<UserInfo>();
            using (MainContext db = new MainContext())
            {
                user = db.UserInfo.Where(o => (o.Username.Contains(search)
                                                  || o.FullName.Contains(search)
                                                  || search == ""
                                                  )
              ).ToList();
            }
            return user;
        }

        public static UserInfoPagination<List<UserInfo>> GetUserPaging(string conStr, int skip, int take, string sort, string fillter)
        {


            string query = @"SELECT 
                            COUNT(*)
                            FROM UserInfo
 
                            SELECT  * FROM UserInfo
                            ORDER BY Username
                            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

            using (var connection = new SqlConnection(conStr))
            {
                var reader = connection.QueryMultiple(query, new { Skip = skip, Take = take });

                int count = reader.Read<int>().FirstOrDefault();
                List<UserInfo> allTodos = reader.Read<UserInfo>().ToList();

                var result = new UserInfoPagination<List<UserInfo>>(allTodos, count, skip, take);
                result.TotalRecords = count;
                return result;
            }



        }
    }
}
