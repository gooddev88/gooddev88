using RobotWasm.Client.Pages.DPMBoard.ML;

namespace RobotWasm.Client.Pages.DPMBoard.A00 {
    public class TileLayoutService {

     async   public static Task<List<TileItem>> ListDefaultTileV() {
            List<TileItem> tt = new List<TileItem>();
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 1,
                Id = "EventLocationMapStreetMap",
                Content = "",
                Title = "",
                Visible = true
            }  );

            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 2,
                Id = "NumberEvent",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 3,
                Id = "NumberInjure",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 3,
                Id = "NumberDead",
                Content = "",
                Title = "",
                Visible = true
            });
     tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 5,
                Id = "NumberDeadRate",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 6,
                Id = "RoadType",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 7,
                Id = "TimeRange",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 8,
                Id = "Vehicle",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 9,
                Id = "AgeRange",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 10,
                Id = "EventCause",
                Content = "",
                Title = "",
                Visible = true
            });
            return tt;

        }
        async public static Task<List<TileItem>> ListDefaultTileH() {
            List<TileItem> tt = new List<TileItem>();
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 1,
                Id = "EventLocationMapStreetMap",
                Content = "",
                Title = "",
                Visible = true
            });

            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 2,
                Id = "NumberEvent",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 3,
                Id = "NumberInjure",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 3,
                Id = "NumberDead",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 5,
                Id = "NumberDeadRate",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 6,
                Id = "RoadType",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 7,
                Id = "TimeRange",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 8,
                Id = "Vehicle",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 9,
                Id = "AgeRange",
                Content = "",
                Title = "",
                Visible = true
            });
            tt.Add(new TileItem {
                RowSpan = 8,
                ColSpan = 4,
                Order = 10,
                Id = "EventCause",
                Content = "",
                Title = "",
                Visible = true
            });
            return tt;

        }
        public Task<List<TileItem>> GetTilesDataAsync() {
        return Task.FromResult(new List<TileItem>
    {
            new TileItem()
            {
                Title = "Total Streams",
                Content = "TotalStreams",
                RowSpan = 1,
                ColSpan = 1
            },
            new TileItem()
            {
                Title = "Total Downloads",
                Content = "TotalDownloads",
                RowSpan = 1,
                ColSpan = 1
            },
            new TileItem()
            {
                Title = "Total Reach",
                Content = "TotalReach",
                RowSpan = 1,
                ColSpan = 1
            },
            new TileItem()
            {
                Title = "Total Revenue",
                Content = "TotalRevenue",
                RowSpan = 1,
                ColSpan = 1
            },
            new TileItem()
            {
                Title = "Weekly Recap-Downloads",
                Content = "WeeklyRecap",
                RowSpan = 2,
                ColSpan = 2
            },
            new TileItem()
            {
                Title = "Performance Trend",
                Content = "PerformanceTrend",
                RowSpan = 2,
                ColSpan = 2
            },
            new TileItem()
            {
                Title = "Top 5 Episodes",
                Content = "TopEpisodes",
                RowSpan = 2,
                ColSpan = 4
            }
    });
    }
}
}
