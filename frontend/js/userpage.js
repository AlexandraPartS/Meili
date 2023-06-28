const uri = '/api/user';
const uri2 = '/api/user';

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function _displayItems(data) {
    const tBody = document.getElementById('usersList');
    tBody.innerHTML = data;
}

// Добавление пользователя
async function createUser(userName, tel) {
    const response = await fetch(uri2, {
        method: "POST",
        headers: { 'Accept': 'application/json', "Content-Type": "application/json" },
        body: JSON.stringify({
            NickName: userName,
            PhoneNumber: tel
        })
    });
    console.log("--------1111---------1111------");
    if (response.ok === true) {
        //const user = await response.json();
        const user = await response.text();
        //document.querySelector("tbody").append(row(user));
        //document.querySelector(".tbody").append(row(user));
        const tBody = document.getElementById('tbody');
        tBody.innerHTML = user;
    }
    else {
        const error = await response.json();
    }
}

// отправка формы
document.getElementById("saveBtn").addEventListener("click", async () => {
    //const id = document.getElementById("userId").value;
    const name = document.getElementById("userName").value;
    const tel = document.getElementById("userTel").value;
    //if (id === "")
        await createUser(name, tel);
    //else
        //await editUser(name, tel);
    reset();
});

getItems();