const uri = 'api/books';
let titles = [];
let book = {};

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function getItemsbyId(id) {
        fetch(uri);
        //.then(response => response.json());
    //     .then(data => console.log(data))
    //     .catch(error => console.error('Unable to get items.', error));
}

function _displayItems(data) {
    titles = data;
    populateList();
}

function _displayBook(data){
    //$("titleId").append(data);
    console.log(data);
}


function populateList() {
    var items = [];
    $.each(titles, function (i, item) {

        items.push('<li><a href=books/GetBookById/' + i + '>' + item + '</a></li>');
        console.log(i, item)

    }); // close each()

    $('#bookId').append(items.join(''));
}