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
    document.getElementById("userName").value = `${data.nickName}`;
    document.getElementById("userTel").value = `${data.phoneNumber}`;
    var avatar = document.getElementById("user-photo");
    let path = document.createTextNode(data.avatar).data;
    if (path === 'null') {
        avatar.src = ``;
    }
    else {
        avatar.src = `..${path}` + `?v=${Math.random()}`;
    }
}

function updateItem() {
    const formElem = document.getElementById("formData");
    let formData = new FormData(formElem);
    let id = document.getElementById("userId").value;
    console.log("Id = "+ id);


    fetch(`${uri}/${id}`, {
        method: 'PUT',
        body: formData,
        processData: false,
        contentType: false
    })
        .then(response => response.json())
        .then((text) => {
            getItem();
            //reset();
        })
        .catch(error => console.error('Unable to post item.', error));
}

function resetAvatar() {
    document.getElementById("user-photo").src = ``;
}


//////////////////////////////////////////////////////////////////

////
////FOR USERS LIST
////


// Get all users
function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

// Create new user
function addItem() {
    const name = document.getElementById("userName").value;
    const tel = document.getElementById("userTel").value;

    const item = {
        NickName: name,
        PhoneNumber: tel
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            reset();
        })
        .catch(error => console.error('Unable to add item.', error));
}

// Edit user data
function displayEditForm(id) {
    const item = userList.find(item => item.id === id);

    document.getElementById('edit-name').value = item.nickName;
    document.getElementById('edit-phoneNumber').value = item.phoneNumber;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('editForm').style.display = 'block';
}

// Delete user
function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function _displayItems(data) {
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

        let td2 = tr.insertCell(2);
        textNode = document.createTextNode(item.phoneNumber);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(3);
        let img = document.createElement('img');
        let path = document.createTextNode(item.avatar).data;
        console.log("My path :" + path);
        img.style = 'position:inherit;top:10px;left:10px;width:100px';
        img.src = `..${path}`;
        td3.appendChild(img);

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let td4 = tr.insertCell(4);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(5);
        td5.appendChild(deleteButton);
    });
    userList = data;
}


// Reset form values
//document.getElementById("resetBtn").addEventListener("click", () => reset());

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

// Reset form data after submit
function reset() {
    document.getElementById("userName").value =
    document.getElementById("userTel").value = "";
}



