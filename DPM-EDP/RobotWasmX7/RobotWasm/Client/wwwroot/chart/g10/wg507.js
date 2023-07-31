



export function wg507(data) {
  


    Highcharts.chart('div_wg507', {
        title: {
            text: ''
        },
        xAxis: data.xaxis,

        yAxis: [{
            title: {
                text: 'จำนวนครั้ง'
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