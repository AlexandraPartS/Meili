const uripic = '/api/pic';
let fileList = [];

// Get all users
function getItems() {
    fetch(uripic)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}


function loadPic() {

    const formElem = document.getElementById("formElem");

    let formData = new FormData(formElem);

    fetch(uripic, {
        method: 'POST',
        body: formData,
        processData: false,
        contentType: false
    })
        .then(response => response.formData())
        .then((text) => {
            //getItems();
            //document.getElementById("fileInfo").innerHTML = text;
            alert(text);
            //reset();
        })
        .catch(error => console.error('Unable to post item.', error));
}
//function loadPic() {

//    const formElem = document.getElementById("formElem");

//    let formData = new FormData(formElem);

//    fetch(uripic, {
//        method: 'POST',
//        body: formData,
//        processData: false,
//        contentType: false
//    })
//        .then(response => response.json())
//        .then((text) => {
//            //getItems();
//            //document.getElementById("fileInfo").innerHTML = text;
//            alert(text);
//            //reset();
//        })
//        .catch(error => console.error('Unable to post item.', error));
//}


function getItem() {

    let img = document.createElement('img');
    img.style = 'position:fixed;top:10px;left:10px;width:100px';
    document.body.append(img);

    fetch(uripic)
        .then(response => response.blob())
        .then((blob) => {
            img.src = URL.createObjectURL(blob);
        })
        .catch(error => console.error('Unable to set item.', error));


}



