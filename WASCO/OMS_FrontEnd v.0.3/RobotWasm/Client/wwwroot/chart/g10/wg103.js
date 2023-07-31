



export function wg103(data) {
  


    Highcharts.chart('div_wg103', {
        title: {
            text: ''
        },
        xAxis: data.xaxis,

        yAxis: [{
            title: {
                text: 'จำนวนผู้ประสบเหตุ'
            },
            showFirstLabel: false
        }],
        plotOptions: {

            line: {
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
        series:  data.series
    });



    
}