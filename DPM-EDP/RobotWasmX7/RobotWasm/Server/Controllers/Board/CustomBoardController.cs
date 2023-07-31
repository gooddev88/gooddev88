using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Server.Data.DA.Board;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using RobotWasm.Shared.Data.ML.DPMBaord.CustomBoard;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Client.Pages.DPMBoard.CustomBoard.Widgets.ThMap.ThMap201;
using static RobotWasm.Server.Data.DA.Board.CustomBoardService;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.Board {
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBoardController : ControllerBase {

        [HttpGet("GetCustomBoard")]
        public CustomBoardDocSet GetCustomBoard(string board_id) {
            var output = GetDocset(board_id);
            return output;
        }

        [HttpGet("ListCustomBoardInUser")]
        public List<vw_custom_board_in_user> ListCustomBoardInUser(string user) {
            var output = CustomBoardService.ListCustomBoardInUser(user);
            return output;
        }
        #region Board management
        [HttpPost("CreateUpdateBoard")]
        public I_BasicResult CreateUpdateBoard([FromBody] custom_board_in_user board) {
            I_BasicResult r = CustomBoardService.CreateUpdateBoard(board);
            return r;
        }
        [HttpGet("RemoveCustomBoard")]
        public I_BasicResult RemoveCustomBoard(string board_id) {
            return CustomBoardService.RemoveCustomBoard(board_id);
        }
        #endregion



        [HttpGet("ListWidgetMasterForAdd")]
        public List<custom_widget_master_select> ListWidgetMasterForAdd(string board_id) {
            var output = CustomBoardService.ListWidgetMasterForAdd(board_id);
            return output;
        }

        [HttpPost("AddWidget")]
        public I_BasicResult AddWidget([FromBody] string data) {
            var widgets = JsonSerializer.Deserialize<List<vw_custom_widget_in_user>>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = CustomBoardService.AddWidget(widgets);
            return r;
        }
      [HttpGet("GetWidgetInfo")]
        public custom_widget_master GetWidgetInfo( string widget_id) {
            return CustomBoardService.GetWidgetInfo( widget_id);
        }
        [HttpPost("SaveWidget")]
        public I_BasicResult SaveWidget([FromBody] string data) {
            var widgets = JsonSerializer.Deserialize<List<vw_custom_widget_in_user>>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            }); 
            I_BasicResult r = CustomBoardService.SaveWidget(widgets);
            return r;
        }
        [HttpGet("RemoveWidget")]
        public I_BasicResult RemoveWidget(string board_id, string widget_id) {
            return CustomBoardService.RemoveWidget(board_id, widget_id);
        }



        #region  parameter

        [HttpGet("GetWidgetParam")]
        public List<custom_widget_param_in_user> GetWidgetParam(string board_id, string widget_id) {
            return CustomBoardService.GetWidgetParam(board_id, widget_id);
        }

        [HttpPost("GetWidgetParam")]
        public I_BasicResult SaveWidgetParam([FromBody]  List<custom_widget_param_in_user> data, [FromQuery] int? save_all_in_group) {
            return CustomBoardService.SaveWidgetParam( data, save_all_in_group);
        }
        #endregion
        
    }
}