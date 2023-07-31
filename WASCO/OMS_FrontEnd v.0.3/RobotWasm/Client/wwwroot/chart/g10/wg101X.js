

export function wg101(data) {
    Highcharts.chart('div_wg101', {
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
            useHTML: true,
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
                    inside: true,
                    useHTML: true
                
                },
                allowPointSelect: true
            }, 
         
        }, 
        series: data.series

    });
}