
export function wg119(data) {



    //Highcharts.chart('div_wg103', {
    //    title: {
    //        text: ''
    //    },
    //    xAxis: data.xaxis,

    //    yAxis: [{
    //        title: {
    //            text: 'จำนวนผู้ประสบเหตุ'
    //        },
    //        showFirstLabel: false
    //    }],
    //    plotOptions: {

    //        line: {
    //            groupPadding: 0,
    //            pointPadding: 0,
    //            dataLabels: {
    //                enabled: true,
    //                align: 'center',
    //                color: '#FFFFFF',
    //                inside: true
    //            },
    //            allowPointSelect: true
    //        },

    //    },
    //    series: data.series
    //});





    Highcharts.chart('div_wg119', {
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: '',
            align: 'left'
        },

        xAxis: data.xaxis,
        yAxis: [{ // Primary yAxis

            title: {
                text: 'ความเสียหาย',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            }  ,   labels: {
                format: '{value} บาท',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: true
        }, { // Secondary yAxis
            title: {
                text: 'จำนวนอุบัติเหตุ',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            labels: {
                
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: false
        }],
        tooltip: {
            shared: true
        },

        series: data.series
    });




    //Highcharts.chart('div_wg119', {
    //    chart: {
    //        type: 'column'
    //    },
    //    title: {
    //        text: ''
    //    },

    //    xAxis: {
    //        type: 'category', 
    //        labels: {
    //            useHTML: true,
    //            animate: true,

    //            style: {
    //                textAlign: 'center'
    //            }
    //        }
    //    },

    //    yAxis: [{
    //        title: {
    //            text: 'จำนวนผู้ประสบเหตุ'
    //        },
    //        showFirstLabel: false
    //    }],
    //    tooltip: {

    //        shared: true,
    //        useHTML: true
    //    },
    //    plotOptions: {

    //        column: {
    //            groupPadding: 0,
    //            pointPadding: 0.1,
    //            dataLabels: {
    //                enabled: true,
    //                align: 'center',
    //                color: '#FFFFFF',
    //                inside: true
    //            },
    //            allowPointSelect: true
    //        },

    //    },
    //    series: data.series

    //});
}