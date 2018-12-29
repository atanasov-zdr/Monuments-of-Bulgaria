function throwAlert(event, entity) {
    let result = confirm('Are you sure you want to delete this ' + entity + '?');
    if (result == false) {
        event.preventDefault();
    }
}