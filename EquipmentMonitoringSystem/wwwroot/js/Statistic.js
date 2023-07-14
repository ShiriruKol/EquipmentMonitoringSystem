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
        var select = document.getElementById("SelectTO");
        var value = select.value;

        $.ajax({
            type: "POST",
            url: "/Statistic/GetStatisticsUplanned",
            data: { 'idmounth': value },
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
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                legend: {
                    display: false,
                },
                responsive: true,
                aspectRatio: 2,
                maintainAspectRatio: true,
               
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

    $("#IDPLAN").click(function () {
        $(this).attr('disabled', true); // атрибут disabled
        var select = document.getElementById("SelectTO");
        var value = select.value;

        $.ajax({
            type: "POST",
            url: "/Statistic/GetStatisticsUplanned",
            data: { 'idmounth': value, 'checkunpl': false },
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

            const ctx3 = document.getElementById('myChart4');

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
    $("#IDSTMONTH").click(function () {
        $(this).attr('disabled', true); // атрибут disabled
        var select = document.getElementById("SelectST");
        var value = select.value;

        $.ajax({
            type: "POST",
            url: "/Statistic/GetStatisticsStation",
            data: { 'idstat': value },
            contextType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessResult,
            error: OnError
        });

        function OnSuccessResult(data) {
            const ctx5 = document.getElementById('myChart5');

            const labels = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];

            new Chart(ctx5,
                {
                    type: 'line',
                    data: data,
                    data: {
                        labels: labels,
                        datasets: [{
                            label: 'Количество тех. обслуживаний по месяцам',
                            data: data,
                            fill: false,
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1
                       
                        }]
                    },
                    
                });
        }

        function OnError(err) {
        }
    });
});