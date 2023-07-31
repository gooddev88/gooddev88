
function SetDotNetHelper201(dotNetHelper) {
    window.dotNetHelper201 = dotNetHelper;
}
function SetDotNetHelper401(dotNetHelper) {
    window.dotNetHelper401 = dotNetHelper;
}
function SetDotNetHelper403(dotNetHelper) {
    window.dotNetHelper403 = dotNetHelper;
}

function ProvinceClick201(province) {
    window.dotNetHelper201.invokeMethodAsync('BlazorProvinceClick201', province);
}
function ProvinceClick401(province) {
    window.dotNetHelper401.invokeMethodAsync('BlazorProvinceClick401', province);
}
function ProvinceClick403(province) {
    window.dotNetHelper403.invokeMethodAsync('BlazorProvinceClick403', province);
}

async function create_201(data) {
    render201(data);
}
async function create_401(data) {
    render401(data);
} 
  async function create_403(data){ 
      render403(data); 
} 
function render201(data) {
    Highcharts.mapChart('map_201', {
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
                            ProvinceClick201(this.custom.province);
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
function render401(data) {
    Highcharts.mapChart('map_401', {
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
                            ProvinceClick401(this.custom.province);
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
function render403(data) {
    Highcharts.mapChart('map_403', {
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
                            ProvinceClick403(this.custom.province);
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


//function openwindow(xurl,province) {
//    if (xurl != '') {
//        alert(province);
//        window.open(xurl, "_self")
//    }
//}



 