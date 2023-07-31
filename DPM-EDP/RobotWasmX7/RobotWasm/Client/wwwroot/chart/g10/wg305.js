
export function wg305(data) {
    Highcharts.chart('div_wg305', {
        chart: {
            type: 'column'
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
                text: 'ผู้ปีประสบการณ์'
            },
            showFirstLabel: false
        }],
        tooltip: {

            shared: true,
            useHTML: true
        },
        plotOptions: {

            column: {
                groupPadding: 0,
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
        series: data.series

    });
}