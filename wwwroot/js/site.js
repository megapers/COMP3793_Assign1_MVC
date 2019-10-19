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
    var url = "https://localhost:5001/api/books/" + id;
    $.get(url, function(data){
        _displayBook(data);
    });
}

function _displayItems(data) {
    titles = data;
    populateList();
}

function _displayBook(data){
    $("#image").attr("src", data.smallThumbnail);
    $("#title").append(data.title);
    $("#publisher").append(data.publisher);
    $("#publishedDate").append(data.publishedDate);
    $("#description").append(data.description);
    $("#isbN_10").append(data.isbN_10_Id);
    
    var authors = data.authors;
    var authorsArray = [];
    $.each(authors, function (i, author) {
            authorsArray.push('<li>' + author +'</li>');
        }); 
    $('#authors').append(authorsArray.join(''));
}

function populateList() {
    var items = [];
    $.each(titles, function (i, item) {

        items.push('<li><a href = Books/GetBookById/' + i + '>' + item + '</a></li>');
        console.log(i, item)

    }); // close each()

    $('#bookId').append(items.join(''));
}