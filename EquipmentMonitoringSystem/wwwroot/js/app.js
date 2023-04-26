(async () => {

    // создаем и покажем уведомление
    const showNotification = () => {

        $.ajax({
            type: "POST",
            url: "/UpcomingMaintenance/CountNortify",
            contextType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessResult,
            error: OnError
        });

        function OnSuccessResult(data) {
            var _data = data;
            // создаем новое уведомление
            const notification = new Notification('Технические обслуживания', {
                body: 'У вас ' + _data + ' непроизведенных(-ый) ремонт(-ов)',
                icon: './Icon.png'
            });

            // закрываем уведомление через 10 секунд
            setTimeout(() => {
                notification.close();
            }, 10 * 1000);

            CheckRepeat = false;
        }

        function OnError(err) {
           
        }

    }

    // отобразим сообщение об ошибке
    const showError = () => {
        const error = document.querySelector('.error');
        error.style.display = 'block';
        error.textContent = 'You blocked the notifications';
    }

    // проверим разрешение на уведомление
    let granted = false;

    if (Notification.permission === 'granted') {
        granted = true;
    } else if (Notification.permission !== 'denied') {
        let permission = await Notification.requestPermission();
        granted = permission === 'granted' ? true : false;
    }

    // покажем уведомление или ошибку
    granted ? showNotification() : showError();

})();