
//load data to chart
function LoadDataToChart() {
   
            $.ajax({
                type: "GET",
                url: "/Admin/Manage/GetChartApm",
                data: "",
                dataType: "json",
                encode: true,
            }).done(function (data) {
                console.log(data);
                if (data.error == 1) {
                }
                if (data.error == 0) {
                    const ctx = document.getElementById('myChart');

                    new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: data.datalabel ,
                            datasets: [
                                {
                                    label: 'Pending',
                                    data: data.pending,
                                    borderWidth: 1,
                                    backgroundColor: "#ffc107"
                                }
                                ,
                                {
                                    label: 'Confirmed',
                                    data: data.confirmed,
                                    borderWidth: 1,
                                    backgroundColor: "#007bff"
                                },
                                {
                                    label: 'Completed',
                                    data: data.completed,
                                    borderWidth: 1,
                                    backgroundColor: "#00ff21"
                                }
                                ,
                                
                                
                                {
                                    label: 'Cancelled',
                                    data: data.cancelled,
                                    borderWidth: 1,
                                    backgroundColor: "#dc3545"
                                }
                            ]
                        },
                        options: {
                            scales: {
                                yAxes: [{
                                    ticks: {
                                        beginAtZero: true,
                                        callback: function (value) { if (Number.isInteger(value)) { return value; } },
                                        stepSize: 1
                                    }
                                   
                                }]
                            },
                            responsive: true,
                            interaction: {
                                intersect: false,
                            },
                        },
                        
                        
                    });


                }
            });
}

$("document").ready(function () {
    LoadDataToChart();

});