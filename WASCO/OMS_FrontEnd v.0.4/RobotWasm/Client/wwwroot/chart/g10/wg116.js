

export function wg116(data) {
    



    Highcharts.chart('div_wg116', {
        title: {
            text: ''
        },
        colorAxis: {
            minColor: '#FFFFFF',
            maxColor: Highcharts.getOptions().colors[0]
        },
        series: data.series
        //series: [{
        //    type: 'treemap',
        //    layoutAlgorithm: 'squarified',
        //    //data: data.series,
        //    data: [{
        //        name: 'A',
        //        value: 6,
        //        colorValue: 10
        //    }, {
        //        name: 'B',
        //        value: 6,
        //        colorValue: 2
        //    }, {
        //        name: 'C',
        //        value: 4,
        //        colorValue: 3
        //    }, {
        //        name: 'D',
        //        value: 3,
        //        colorValue: 4
        //    }, {
        //        name: 'E',
        //        value: 2,
        //        colorValue: 5
        //    }, {
        //        name: 'F',
        //        value: 2,
        //        colorValue: 6
        //    }, {
        //        name: 'G',
        //        value: 1,
        //        colorValue: 7
        //    }]
        //}]
       
    });
}