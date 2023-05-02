$(document).ready(function () {

    $('#btndown').on('click', function () {

        if (document.getElementById("btnradio1").checked) {
            $.ajax({
                type: "POST",
                url: "/UpcomingMaintenance/UnplanAll",
                contextType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessResult,
                error: OnError
            });

            function OnSuccessResult(data) {
                var _data = data;
                var heads = _data[0];
                var descs = _data[1];
                var mainidlist = _data[2];
                var eqlist = _data[3];
                var stlist = _data[4];

                if (Object.keys(heads).length !== 0) {
                    document.getElementById('noinfo').hidden = true;
                    document.getElementById('yesinfo').hidden = false;

                    $("#info-table").find("tr:gt(0)").remove();//удаление старых данных*/

                    for (i = 0; i < Object.keys(heads).length; i++) {
                        $(`#info-table tbody`).prepend('<tr><td>'
                            + heads[i] + '</td><td>'
                            + descs[i] + '</td><td>'
                            + eqlist[i] + '</td><td>'
                            + stlist[i]
                            + '</td><td><a class="btn btn-success" href="/UpcomingMaintenance/Fix/' + mainidlist[i] + '" title="Нажмите, чтобы объявить о прохождении ТО."><i class="fa - sharp fa - solid fa - check"></i>Выполнить</a></td></tr>');
                    }
                } else {
                    document.getElementById('noinfo').hidden = false;
                    document.getElementById('yesinfo').hidden = true;
                }
                
            }

            function OnError(err) {
                alert("Произошла ошибка!!!");
            }

        } else if (document.getElementById("btnradio2").checked) {

            var select = document.getElementById("SelectedGroupId");
            var value = select.value;
            if (value == 0) {

                document.getElementById('alert').hidden = false;

            } else {

                document.getElementById('alert').hidden = true;

                $.ajax({
                    type: "POST",
                    url: "/UpcomingMaintenance/PlanAll",
                    data: { 'grid': value },
                    contextType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccessResult,
                    error: OnError
                });

                function OnSuccessResult(data) {

                    var _data = data;
                    var heads = _data[0];
                    var descs = _data[1];
                    var mainidlist = _data[2];
                    var eqlist = _data[3];
                    var stlist = _data[4];

                    $("#info-table").find("tr:gt(0)").remove(); //удаление старых данных

                    
                    if (typeof heads !== 'undefined') {
                        document.getElementById('noinfo').hidden = true;
                        document.getElementById('yesinfo').hidden = false;

                        for (i = 0; i < Object.keys(heads).length; i++) {
                            $(`#info-table tbody`).prepend('<tr><td>'
                                + heads[i] + '</td><td>'
                                + descs[i] + '</td><td>'
                                + eqlist[i] + '</td><td>'
                                + stlist
                                + '</td><td><a class="btn btn-success" href="/UpcomingMaintenance/Fix/' + mainidlist[i] + '" title="Нажмите, чтобы объявить о прохождении ТО."><i class="fa - sharp fa - solid fa - check"></i>Выполнить</a></td></tr>');
                        }
                    } else {
                        document.getElementById('noinfo').hidden = false;
                        document.getElementById('yesinfo').hidden = true;
                    }
                    
                }

                function OnError(err) {
                    alert("Произошла ошибка!!!");
                }
            }
        }

    });

    $("#btnradio2").change(function () {
        document.getElementById('divselect').hidden = false;

        var select = document.getElementById("SelectedStationId");
        var value = select.value;

        $.ajax({
            type: "POST",
            url: "/Equipment/StationSelect",
            contextType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessResult,
            error: OnError
        });

        function OnSuccessResult(data) {
            var _data = data;
            var idgrs = _data[0];
            var namegrs = _data[1];

            var selectst = document.getElementById("SelectedStationId");
            $('#SelectedStationId').find('option').remove(); //удаление старых данных

            let selected = selectst.options[selectst.selectedIndex];

            let newOpt = new Option('Выберите станцию', 0);
            newOpt.setAttribute('disabled', '');
            selectst.append(newOpt);
            newOpt.selected = true;


            for (var i = 0; i < idgrs.length; i++) {
                let selectedOption = selectst.options[selectst.selectedIndex];

                let newOption = new Option(namegrs[i], idgrs[i]);
                selectst.append(newOption);
            }
        }

        function OnError(err) {
            alert("Произошла ошибка!!!")
        }
    });

    $("#SelectedStationId").change(function () {

        var select = document.getElementById("SelectedStationId");
        var value = select.value;

        $.ajax({
            type: "POST",
            url: "/Equipment/GroupSelect",
            data: { 'stid': value },
            contextType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessResult,
            error: OnError
        });

        function OnSuccessResult(data) {
            var _data = data;
            var idgrs = _data[0];
            var namegrs = _data[1];

            var selectgr = document.getElementById("SelectedGroupId");

            $('#SelectedGroupId').find('option').remove(); //удаление старых данных

            let selected = selectgr.options[selectgr.selectedIndex];

            let newOpt = new Option('Выберите группу', 0);
            newOpt.setAttribute('disabled', '');
            selectgr.append(newOpt);
            newOpt.selected = true;


            for (var i = 0; i < idgrs.length; i++) {

                let selectedOption = selectgr.options[selectgr.selectedIndex];

                let newOption = new Option(namegrs[i], idgrs[i]);
                selectgr.append(newOption);

            }
        }

        function OnError(err) {
            alert("Произошла ошибка!!!")
        }
        return false;
    });


    $("#btnradio1").change(function () {
        document.getElementById('divselect').hidden = true;
    });


});