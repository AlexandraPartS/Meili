const uri = '/api/user';
const uriid = '/api/user/52';
let userList = [];

////
////FOR ONE USER
////

// Get user
function getItem() {
    fetch(uriid)
        .then(response => response.json())
        .then(data => _displayItem(data))
        .catch(error => console.error('Unable to get items.', error));
}

function _displayItem(data) {
    document.getElementById("userId").value = `${data.id}`;
    document.getElementById("NickName").value = `${data.nickName}`;
    document.getElementById("PhoneNumber").value = `${data.phoneNumber}`;
    var avatar = document.getElementById("user-photo");
    let path = document.createTextNode(data.avatarRelativePath).data;
    if (path === 'null') {
        avatar.src = ``;
    }
    else {
        avatar.src = `..${path}` + `?v=${Math.random()}`;
    }
}

function updateItem() {

    //var1
    const formElem = document.getElementById("formData");
    let formData = new FormData(formElem);
    let id = document.getElementById("userId").value;
    //console.log("Id = " + id);

    formData.forEach(item => {
        console.log(typeof item + ", " + item + ", ");

    });


    //var2
    //const id = document.getElementById('userId').value;
    //const item = {
    //    Id: Number(id),
    //    NickName: document.getElementById('NickName').value.trim(),
    //    PhoneNumber: document.getElementById('PhoneNumber').value.trim()
    //};
    //console.log("item = " + item.Id + item.NickName);

    //var3
    //let formData = new FormData();
    //let id = document.getElementById("userId").value;
    ////const id = document.getElementById('userId').value;
    //const item = {
    //    Id: Number(id),
    //    NickName: document.getElementById('NickName').value.trim(),
    //    PhoneNumber: document.getElementById('PhoneNumber').value.trim()
    //};
    //var input = document.getElementById("userAvatar");
    //var file = input.files[0];
    //formData.append("postedFile", file);
    //formData.append("userDto", JSON.stringify(item));
    ////formData.append("Id", 3);

    formData.forEach(item => {
        console.log(typeof item + ", " + item + ", " + item.name);
    });


    //fetch(`${uri}`, {
    fetch(`${uri}/${id}`, {
        method: 'PUT',
        //body: JSON.stringify(item),
        body: formData//,
        //processData: false,
        //contentType: false,
    //    headers: {
    //        'Content-Type': 'multipart/form-data',
    //        //'Content-Type': 'application/json',
    //        //'Authorization': `Bearer ${getTokenFromCookie()}`,
    //        'Accept': 'application/json, application/xml, text/plain, text/html, *.*'
    //    }
    })
        .then(response => response.json())
        .then((text) => {
            getItem();
            //reset();
        })
        .catch(error => console.error('Unable to post item.', error));
}


//function updateItem() {

//    let formData = new FormData();

//    const id = document.getElementById('userId').value;
//    const item = {
//        Id: Number(id),
//        NickName: document.getElementById('NickName').value.trim(),
//        PhoneNumber: document.getElementById('PhoneNumber').value.trim()
//    };
//    formData.append("userDto", JSON.stringify(item));

//    var input = document.getElementById("userAvatar");
//    var file = input.files[0];
//    formData.append("files", file);
    
//    console.log("JSON.stringify(item) = " + JSON.stringify(item));

//    fetch(`${uri}/${id}`, {
//        method: 'PUT',
//        body: formData,
//        processData: false,
//        contentType: false
//    })
//        .then(response => response.json())
//        .then((text) => {
//            getItem();
//            //reset();
//        })
//        .catch(error => console.error('Unable to post item.', error));
//}


//function updateItem() {

//    const formElem = document.getElementById("formData");
//    let formData = new FormData(formElem);
//    let id = document.getElementById("userId").value; 
//    console.log("Id = " + id);

//    fetch(`${uri}/${id}`, {
//        method: 'PUT',
//        body: formData,
//        processData: false,
//        contentType: false
//    })
//        .then(response => response.json())
//        .then((text) => {
//            getItem();
//            //reset();
//        })
//        .catch(error => console.error('Unable to post item.', error));
//}

function resetAvatar() {
    document.getElementById("user-photo").src = ``;
    document.getElementById("userAvatar").value = "";
}
