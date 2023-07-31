 
  async function create_chart(data){ 
    render(data); 
}

function SetDotNetHelper(dotNetHelper) {
    window.dotNetHelper = dotNetHelper;
}

function ProvinceClick(province) {
     
    window.dotNetHelper.invokeMethodAsync('BlazorProvinceClick', province);
}

  
function render(data) {
    Highcharts.mapChart('map_thai', {
        chart: {
            map: 'countries/th/th-all'
        },
        title: {
            text: ''
        },
        mapNavigation: {
            enabled: false,
            enableDoubleClickZoomTo: false,
            enableMouseWheelZoom: true,

            buttonOptions: {
                verticalAlign: 'top'
            }
        },

        plotOptions: {
            series: {
                cursor: 'pointer',
                point: {
                    events: {
                        click: function () {
                            ProvinceClick(this.custom.province);
                            //openwindow(this.custom.url,this.custom.province);
                        }
                    }
                }
            }
        },
        // Limit zoom
        xAxis: {
            minRange: 0
        },

        // We do not want a legend
        legend: {
            enabled: false
        },
        tooltip: {
            useHTML: true,
            enabled: false,
            backgroundColor: 'white',
            borderRadius: 40,
            borderColor: '#aaa',
            headerFormat: '<strong style="font-size: 0.9rem;font-family: Sarabun,serif;color: black">จังหวัด {point.point.custom.province}</strong><br>',
            pointFormat: '<span style="font-size: 0.9rem;font-family: Sarabun,serif;color: black">{point.options.custom.memo}</span>'
        },


        // Define the series
        series: [{
            name: 'ระบบแจ้งเตือนสาธารณภัย',
            keys: [
                //'hc-key', 'custom.url', 'color.pattern.image', 'color.pattern.color', 'custom.memo', 'custom.province'
                'hc-key', 'color.pattern.color', 'custom.province', 'custom.rowno'
            ],
            joinBy: 'hc-key',
            data: data,
            borderColor: '#fff',
            color: {
                pattern: {
                    aspectRatio: 1
                }
            },
            states: {
                hover: {
                    borderColor: '#b44',
                    borderWidth: 2
                }
            },
            dataLabels: {
                useHTML: false,
                enabled: true,
                format: '<span style="font-size: 0.5rem;font-family: Sarabun,serif">{point.name}</span>'
            }
        }, {
            /* Separator lines */
            type: 'mapline',
            nullColor: '#aaa'

        }]
    });

}

function openwindow(xurl,province) {
    if (xurl != '') {
        alert(province);
        window.open(xurl, "_self")
    }
}



 