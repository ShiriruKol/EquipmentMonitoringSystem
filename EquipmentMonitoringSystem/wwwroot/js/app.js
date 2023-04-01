(async () => {
    // создаем и покажем уведомление
    const showNotification = () => {
        // создаем новое уведомление
        const notification = new Notification('Добро пожаловать!!!', {
            body: '(@_@)',
            icon: './img/js.png'
        });

        // закрываем уведомление через 10 секунд
        setTimeout(() => {
            notification.close();
        }, 10 * 1000);

        // переход к URL-адресу при нажатии
        /*notification.addEventListener('click', () => {

            window.open('https://habr.com/ru/all/', '_blank');
        });*/
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