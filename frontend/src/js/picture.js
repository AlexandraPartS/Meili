const uripic = '/api/picture';
let fileList = [];

function loadPic() {

    const formElem = document.getElementById("formElem");

    let formData = new FormData(formElem);
    let id = document.getElementById("userId").value;
    console.log("ID is = "+ id);
    formData.append("Id", id);

    fetch(uripic, {
        method: 'POST',
        body: formData,
        processData: false,
        contentType: false
    })
        .then(response => response.json())
        .then((text) => {
            getItem();
            console.log("We are there");
            //reset();
        })
        .catch(error => console.error('Unable to post item.', error));
}


////
////FOR picture.html
////

function loadPicPicPage() {
    const formElem = document.getElementById("formElem");
    let formData = new FormData(formElem);

    fetch(uripic, {
        method: 'POST',
        body: formData,
        processData: false,
        contentType: false
    })
        .then(response => response.json())
        .then((text) => {
            console.log("We are there");
        })
        .catch(error => console.error('Unable to post item.', error));
}

function getBlobItem() {

    let img = document.createElement('img');
    img.style = 'position:fixed;top:10px;left:10px;width:100px';
    document.body.append(img);

    fetch(uripic)
        .then(response => response.blob())
        .then((blob) => {
            console.log(blob);
            img.src = URL.createObjectURL(blob);
            console.log('LOG');
        })
        .catch(error => console.error('Unable to set item.', error));
}



