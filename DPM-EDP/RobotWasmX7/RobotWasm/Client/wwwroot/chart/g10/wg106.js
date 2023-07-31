
export function wg106(data) {
    Highcharts.chart('div_wg106', {
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
                text: 'ผู้เสียชีวิต'
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