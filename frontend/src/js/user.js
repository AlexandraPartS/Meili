const uriapi = '/api/user';
const uriid = '/api/user/2';
const nickName = document.getElementById('NickName');
const phoneNumber = document.getElementById('PhoneNumber');
const avatarimage = document.getElementById("user-photo");
const avatarimage_s = document.getElementById("user-photo-s");
const avatarfile = document.getElementById("userAvatar");
const submitbtn = document.getElementById("saveBtn");
let isAvatarChanged = false;
let userStartData = {
    NickName: "",
    PhoneNumber: "",
};
const alertdeltatime = 4000;
const alertdeltatime_s = 500;

function getItem() {
    fetch(uriid)
        .then(response => response.json())
        .then(data => _displayItem(data))
        .catch(error => console.error('Unable to get items.', error));
}

function _displayItem(data) {
    document.getElementById("userId").value = `${data.id}`;
    nickName.value = `${data.nickName}`;
    phoneNumber.value = `${data.phoneNumber}`;

    let path = document.createTextNode(data.avatarRelativePath).data;
    if (path === 'null')
    {
        avatarimage.src = ``;
    }
    else {
        avatarimage.src = avatarimage_s.src = `..${path}` + `?v=${Math.random()}`;
    }
    fillUserStartData(nickName.value, phoneNumber.value);
}

async function updateItem() {
    const formElem = document.getElementById("formData");
    let formData = new FormData(formElem);
    let id = document.getElementById("userId").value;

    if (avatarfile.value == "" && avatarfile.files[0] == null && avatarimage.src != '') {
        let imageBlob = await fetch(avatarimage.src).then(r => r.blob());
        formData.append("image", imageBlob, "image.png");
    }

    fetch(`${uriapi}/${id}`, {
        method: 'PUT',
        body: formData
    })
        .then(response => {
            if (response.ok) {
                return response.json()
            }
            throw new Error(response.status);
        })
        .then((text) => {
            getItem();
            reset();
            setTimeout(alertSuccessTimeout("alert-success"), alertdeltatime_s)
        })
        .catch(error => {
            console.error('Unable to add item.', error);
            setTimeout(alertSuccessTimeout("alert-danger"), alertdeltatime_s);
        });
}

function fillUserStartData(nickName, phoneNumber){
    userStartData.NickName = nickName;
    userStartData.PhoneNumber = phoneNumber;
}

if (avatarfile) {
    avatarfile.addEventListener('change', function () {
        if (!!avatarfile.files[0]) {
            avatarimage.src = avatarimage_s.src = URL.createObjectURL(avatarfile.files[0]);
            isAvatarChanged = true;
            activateSubmitButton();
        }
    });
}
function resetAvatar() {
    avatarimage.removeAttribute('src');
    avatarimage_s.removeAttribute('src');
    avatarfile.value = "";
    isAvatarChanged = true;
    activateSubmitButton();
}



/// <summary>
/// Logic for the "Create new user" block of 2 page (userdata.html, userlist.html)
/// Use variables: nickName, phoneNumber, submitbtn;
/// </summary>

function validateDataElement(element) {
    if (Object.is(element, nickName)) {
        return (nickName.value.length >= 2 && nickName.value.length <= 128) ? true : false;
    }
    if (Object.is(element, phoneNumber)) {
        let regex = /^\+(?:[\s\-]?[0-9]●?){6,14}[0-9]$/;
        //let regex = /^\+\d{1,3}[\s\-]?\d{1,14}([\s\-]?\d{1,13})?/;
        return regex.test(phoneNumber.value) ? true : false;
    }
}
nickName.onblur = phoneNumber.onblur = function () {
    !validateDataElement(this) ? setErrorState(this) : activateSubmitButton();
}   
nickName.onfocus = phoneNumber.onfocus = function(){
    this.classList.contains('is-invalid') ? this.classList.remove('is-invalid') : null;
}   
function setErrorState(element) {
    element.classList.add('is-invalid');
    submitbtn.classList.add('disabled');
}
function validateData() {
    return (validateDataElement(nickName) && validateDataElement(phoneNumber)) ? true : false;
}

function activateSubmitButton() {
    //for userdata page
    const formElem = !!document.getElementById("formData");
    if (formElem) {
        if (isAvatarChanged || userStartData.NickName != nickName.value || userStartData.PhoneNumber != phoneNumber.value) {
            if (userStartData.NickName != nickName.value || userStartData.PhoneNumber != phoneNumber.value) {
                if (validateData()) {
                    submitbtn.classList.remove('disabled');
                }
            }
            else {
                submitbtn.classList.remove('disabled');
            }
        }
        else {
            submitbtn.classList.add('disabled');
        }
        return;
    }
    //for userslist page
    validateData() ? submitbtn.classList.remove('disabled') : submitbtn.classList.add('disabled');
};


submitbtn.addEventListener('click', function (e) {
    submitbtn.classList.add('disabled');
});

function alertSuccessTimeout(alertmessagetype) {
    let alertmessage = document.getElementsByClassName("alert " + `${alertmessagetype}`)[0];
    alertmessage.classList.remove('d-none');
    setTimeout(() => {
        alertmessage.classList.add('d-none');
    }, alertdeltatime);
}
function reset() {
    nickName.value =
        phoneNumber.value = "";
    submitbtn.classList.add('disabled');
}