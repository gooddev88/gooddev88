
export function wg401(data) {
    Highcharts.chart('div_wg401', {
 
        chart: {
            type: 'pie'
        },
        title: {
            text: ''
        },

     
        tooltip: {

            shared: true,
            useHTML: true
        },
        //plotOptions: {

        //    column: {
        //        groupPadding: 0,
        //        pointPadding: 0,
        //        dataLabels: {
        //            enabled: true,
        //            align: 'center',
        //            color: '#FFFFFF',
        //            inside: true
        //        },
        //        allowPointSelect: true
        //    }, 
        //},


        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.name}: {y} รายการ'
                },
                showInLegend: true
            }
        },


        series: data.series

    });
}