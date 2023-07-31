﻿

export function wg106(data) {
    Highcharts.chart('div_wg106', {
        chart: {
            type: 'column'
        },
        title: {
            text: ''
        },

        xAxis: data.xaxis,
        yAxis: {
            title: {
                useHTML: true,

            }
        },
        tooltip: {

            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        //series:
        //    [
        //        {
        //            name: 'Oil and gas extraction',
        //            data: [13.93, 13.63, 13.73, 13.67]

        //        }
        //        , {
        //            name: 'Manufacturing industries and mining',
        //            data: [12.24, 12.24, 11.95, 12.02]

        //        }
        //        , {
        //            name: 'Road traffic',
        //            data: [10.00, 9.93, 9.97, 10.01]

        //        }
        //    ]
        series: data.series

    });
}