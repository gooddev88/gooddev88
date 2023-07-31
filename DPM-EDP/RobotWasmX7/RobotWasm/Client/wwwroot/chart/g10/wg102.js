

export function wg102(data) {
    Highcharts.chart('div_wg102', {
        chart: {
            type: 'bar'
        },
        title: {
            text: ''
        },

        xAxis: {
            type: 'category',
          
            labels: {
                useHTML: true,
                animate: true,

                style: {
                    textAlign: 'center'
                }
            }
        },

        yAxis: [{
            title: {
                text: 'จำนวนอุบัติเหตุ'
            },
            showFirstLabel: false
        }],
        tooltip: {

            shared: true,
            useHTML: true
        },
        plotOptions: {

            bar: {
                groupPadding: 0.0,
                pointPadding: 0,
                dataLabels: {
                    enabled: true,
                    align: 'center',
                    color: '#FFFFFF',
                    inside: true
                },
                allowPointSelect: true
            },

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