$(function () {
    $("#ID").click(function () {
        $(this).attr('disabled', true); // атрибут disabled

        $.ajax({
            type: "POST",
            url: "/Statistic/GetStatistics",
            data: "",
            contextType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessResult,
            error: OnError
        });

        function OnSuccessResult(data) {

            var _data = data;
            var _chartLabels = _data[0];
            var _chartData = _data[1];

            var barColor = ["Crimson", "MediumSeaGreen", "DeepSkyBlue", "DarkOrange", "PeachPuff", "MediumPurple"];

            const ctx3 = document.getElementById('myChart3');

            var chartOptions = {
                legend: {
                    display: false
                },
                responsive: true,
                aspectRatio: 2,
                maintainAspectRatio: true,
            };

            

            new Chart(ctx3,
                {
                    type: "pie",
                    data: {
                        labels: _chartLabels,
                        datasets: [{
                            backgroundColor: barColor,
                            data: _chartData,
                            borderWidth: 1
                        }]
                    },
                    options: chartOptions,
                });
        }

        function OnError(err) {

        }
    });
});



$(function () {
    $("#IDUNP").click(function () {
        $(this).attr('disabled', true); // атрибут disabled

        $.ajax({
            type: "POST",
            url: "/Statistic/GetStatisticsUplanned",
            data: "",
            contextType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessResult,
            error: OnError
        });

        function OnSuccessResult(data) {

            var _data = data;
            var _chartLabels = _data[0];
            var _chartData = _data[1];

            var barColor = ["Crimson", "MediumSeaGreen", "DeepSkyBlue", "DarkOrange", "PeachPuff", "MediumPurple"];

            const ctx3 = document.getElementById('myChart2');

            var chartOptions = {
                legend: {
                    display: false
                },
                responsive: true,
                aspectRatio: 2,
                maintainAspectRatio: true,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            };

            new Chart(ctx3,
                {
                    type: "pie",
                    data: {
                        labels: _chartLabels,
                        datasets: [{
                            backgroundColor: barColor,
                            data: _chartData,
                            borderWidth: 1
                        }]
                    },
                    options: chartOptions,
                });
        }

        function OnError(err) {

        }
    });
});



$(function () {
    $("#IDMOUNTH").click(function () {
        $(this).attr('disabled', true); // атрибут disabled

        $.ajax({
            type: "POST",
            url: "/Statistic/GetStatisticsMounth",
            data: "",
            contextType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessResult,
            error: OnError
        });

        function OnSuccessResult(data) {

            var _data = data;
            var _chartLabels = _data[0];
            var _chartData = _data[1];

            var barColor = ["Crimson", "MediumSeaGreen", "DeepSkyBlue", "DarkOrange", "PeachPuff", "MediumPurple"];

            const ctx3 = document.getElementById('myChart1');

            var chartOptions = {
                legend: {
                    display: false
                },
                responsive: true,
                aspectRatio: 2,
                maintainAspectRatio: true,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            };

            new Chart(ctx3,
                {
                    type: "bar",
                    data: {
                        labels: _chartLabels,
                        datasets: [{
                            backgroundColor: barColor,
                            data: _chartData,
                            borderWidth: 1
                        }]
                    },
                    options: chartOptions,
                });
        }

        function OnError(err) {

        }
    });
});