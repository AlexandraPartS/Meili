/// <summary>
/// Logic for the "Create new user" block in the script "user.js":
/// 1. variables nickName, phoneNumber, submitbtn;
/// 2. functions validation - onblur, onfocus, activateSubmitButton, reset
/// </summary>

const uri = '/api/user';
let userList = [];

function updateItemManage() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        Id: Number(itemId),
        NickName: document.getElementById('edit-name').value.trim(),
        PhoneNumber: document.getElementById('edit-phoneNumber').value.trim()
    };
    if (isValidateNickName && isValidatePhone)
    {
        fetch(`${uri}/${itemId}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(() => getItems())
            .catch(error => console.error('Unable to update item.', error));
    }
    closeInput();
    return false;
}


function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const name = document.getElementById("NickName").value;
    const tel = document.getElementById("PhoneNumber").value;

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
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            throw new Error(response.status);
        })
        .then(() => {
            getItems();
            reset();
            setTimeout(alertSuccessTimeout("alert-success"), alertdeltatime_s)
        })
        .catch(error => {
            console.error('Unable to add item.', error);
            setTimeout(alertSuccessTimeout("alert-danger"), alertdeltatime_s);
            setTimeout(() => { reset(); }, alertdeltatime);
        });
}

function displayEditForm(id) {
    const item = userList.find(item => item.id === id);

    document.getElementById('edit-name').value = item.nickName;
    document.getElementById('edit-phoneNumber').value = item.phoneNumber;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('editForm').style.display = 'block';
}

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
    button.classList.add('btn');
    const avatarimg = document.createElement('img');
    avatarimg.setAttribute("width", "50px");
    avatarimg.setAttribute("height", "50px");

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
        let img = avatarimg.cloneNode(false);
        let path = item.avatarRelativePath;
        if (path != null) {
            img.src = `..${path}`;
            td3.appendChild(img);
        }
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.classList.add('btn-secondary');
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.classList.add('btn-danger');
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let td4 = tr.insertCell(4);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(5);
        td5.appendChild(deleteButton);
    });
    userList = data;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}