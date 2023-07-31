using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Server.Data.DA.Board;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.Board {
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase {
 
        [HttpGet("ListBoardInUser")]
        public List<vw_board_in_user_select> ListBoardInUser(string user ) {

            var output= BoardService.ListBoardInUser(user);
            return output;
             
        }

        [HttpGet("GetMasterBoardMenu")]
        public List<vw_board_master> GetMasterBoardMenu() {

            var output = BoardService.GetMasterBoardMenu();
            return output;

        }
        [HttpGet("GetDefaultBoard")]
        public vw_board_master? GetDefaultBoard() {
            return BoardService.GetDefaultBoard();  
        }



        ////topadd
        //[HttpGet("ListApiMasterByCate")]
        //public List<api_master> ListApiMasterByCate(string? cate) {
        //    cate = cate == null ? "" : cate;

        //    var output = BoardService.ListApiMasterByCate(cate);
        //    return output;

        //}
        //[HttpPost("SaveEditCustomBoard")]
        //public I_BasicResult SaveEditCustomBoard(   custom_board_in_user data) {
        //    I_BasicResult r = BoardService.AddEditCustomBoard(data);
        //    return r;

        //}



        [HttpGet("AddUserInBoard")]
        public  I_BasicResult AddUserInBoard(string user,string board) {
            I_BasicResult r= CustomBoardService.AddUserInBoard(user, board); 
            return r;

        }
        [HttpGet("RemoveUserInBoard")]
        public I_BasicResult RemoveUserInBoard(string user, string board) {
            I_BasicResult r = BoardService.RemoveUserInBoard(user, board);
            return r;

        }

          
        [HttpGet("GetUserInWidget")]
        public List<vw_widget_in_user_select> GetUserInWidget(string user,string board,string orientation) {

            var output = BoardService.GetUserInWidget(user, board, orientation);
            return output;

        }

    

        [HttpPost("SaveWidget")]
        public I_BasicResult SaveWidget([FromBody] string data) {
            

            
          var   widgets = JsonSerializer.Deserialize<List<vw_widget_in_user_select>>(  data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = BoardService.SaveWidget(widgets);
            return r;

        }

        [HttpPost("Fuck")]
        public I_BasicResult Fuck(string? xxx ) {
            var xx = xxx;
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return r;

        }
        //[HttpGet("RemoveUserInWidget")]
        //public I_BasicResult RemoveUserInWidget(string user, string board, string widget, string orientation) {
        //    I_BasicResult r = BoardService.RemoveUserInWidget(user, board, widget);
        //    return r;

        //}
    }
}
