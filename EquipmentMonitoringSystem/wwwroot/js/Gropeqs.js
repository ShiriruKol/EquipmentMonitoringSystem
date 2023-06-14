$(document).ready(function () {
    // Выбираем все кнопки на странице и получаем массив
    var btns = document.querySelectorAll('button')
    // Проходим по массиву
    btns.forEach(function (btn) {
        // Вешаем событие клик
        btn.addEventListener('click', function (e) {
            document.getElementById('GroupsIdCol').style.display = ''; // show
            if (btn.id.indexOf('st') != -1) {
                var value = btn.id.replace('st', '');
                $.ajax({
                    type: "POST",
                    url: "/UpcomingMaintenance/SelGroupsCount",
                    data: { 'stid': value },
                    contextType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccessResult,
                    error: OnError
                });

                function OnSuccessResult(data) {
                    var _data = data;
                    let divmain = document.getElementById('GroupsIdCol');
                    divmain.innerText = '';
                    for (let i = 0; i < _data[0].length; i++) {

                        const elementDiv = document.createElement("div");
                        elementDiv.className = "col-3 rounded-3 border m-1";
                        var innerHtml = '<a id="gr' + _data[0][i] + '" href="/UpcomingMaintenance/EqsGroup/' + Number(_data[0][i]) + '" class="btn btn-light text-start" title = "Нажмите, чтобы посмотреть доп.информацию." > Группа: <b>' + _data[1][i] + '</b><br> Кол-во ремонтов: <b class="text-danger">' + _data[2][i] + '</b > </a>';
                        elementDiv.innerHTML = innerHtml;
                        divmain.appendChild(elementDiv);
                    }
                    elementUpdate('GroupsIdCol');
                }

                function OnError(err) {
                    alert("Произошла ошибка!!!");
                }
            }

        })
    })
});