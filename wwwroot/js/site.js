window.addEventListener('load', function () {
    console.log('Trang web đã tải xong.');
});

document.getElementById("serach_data").addEventListener('submit', (e) => {
    e.preventDefault()
    console.log(e)
})