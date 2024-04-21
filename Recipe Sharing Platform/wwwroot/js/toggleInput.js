function toggleInput(id) {
    var containerElement = document.getElementById(id);

    if (containerElement.style.display === 'none') {

        containerElement.style.display = 'block';

    } else {

        containerElement.style.display = 'none';
    }
}
