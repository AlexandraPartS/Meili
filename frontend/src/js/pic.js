const uripic = '/api/pic';
let fileList = [];

function loadPic() {

    const formElem = document.getElementById("formElem");

    let formData = new FormData(formElem);
    //let formData2 = new FormData(formElem2);
    //formData. .append(formData2);
    let id = document.getElementById("userId").value;
    console.log("ID is = "+ id);
    formData.append("Id", id);
    //formData.append("NickName", "Sam");
    //formData.append("PhoneNumber", "4444");

    fetch(uripic, {
        method: 'POST',
        body: formData,
        processData: false,
        contentType: false
    })
        .then(response => response.json())
        //.then(response => response.formData())
        .then((text) => {
            getItem();
            //document.getElementById("fileInfo").innerHTML = text;
            //alert(text);
            console.log("We are there");
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


// Get all users
//function getItems() {
//    fetch(uripic)
//        //.then(response => response.json())
//        .then(response => response.formData())
//        .then(data => _displayItems(data))
//        .catch(error => console.error('Unable to get items.', error));
//}

function _displayItems2(data) {
    console.log("_________________");
    const tBody = document.getElementById('usersList');
    tBody.innerHTML = '';
    const button = document.createElement('button');

    


    data.forEach(item => {

        let tr = tBody.insertRow();

        let td0 = tr.insertCell(0);
        let textNode = document.createTextNode(item.id);
        td0.appendChild(textNode);

        let td1 = tr.insertCell(1);
        textNode = document.createTextNode(item.nickName);
        td1.appendChild(textNode);

        //let td2 = tr.insertCell(2);
        //textNode = document.createTextNode(item.phoneNumber);
        //td2.appendChild(textNode);

        let img = document.createElement('img');
        img.style = 'position:fixed;top:10px;left:10px;width:100px';
        td2.appendChild(img);
        img.src = URL.createObjectURL(item.blob);

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let td3 = tr.insertCell(3);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(4);
        td4.appendChild(deleteButton);
    });

    userList = data;
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



