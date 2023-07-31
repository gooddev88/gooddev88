namespace RobotWasm.Client.Pages.DPMBoard.ML {
    public class TileItem {
        public int RowSpan { get; set; }
        public int ColSpan { get; set; }
        public int Order { get; set; }
        public string Id { get; set; }
        public bool Visible { get; set; } = true;
        public string Title { get; set; }
        public string Content { get; set; }

 
    }
}
