
export function wg113(data) {

     

    Highcharts.chart('div_wg113', {
        chart: {
            type: 'area'
        },
        accessibility: {
            description: ''
        },
        title: {
            text: ''
        },
        subtitle: {
            text: ''
        },
        xAxis: data.xaxis,
        yAxis: {
            title: {
                text: 'จำนวนอุบัติเหตุ'
            } 
        },
        tooltip: {
            pointFormat: 'อุบัติเหตุ <b>{point.y:,.0f}</b>'
        },
        plotOptions: {
            area: {
                
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: data.series
    });
     
     
}