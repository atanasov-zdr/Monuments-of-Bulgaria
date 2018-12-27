function throwAlert(event) {
    let result = confirm('Are you sure you want to delete this trip?');
    if (result == false) {
        event.preventDefault();
    }
}