function userRegisteration() {
    debugger;
    var data = {};
    var profilePicture = document.getElementById("profilePicture").files;
    data.FirstName = $('#firstName').val();
    data.LastName = $('#lastName').val();
    data.GenderId = $('#genderId').val();
    data.Email = $('#email').val();
    data.PhoneNumber = $('#phoneNumber').val();
    data.Password = $('#password').val();
    data.ConfirmPassword = $('#confirmPassword').val();
    if (profilePicture[0] != null) {
        const reader = new FileReader();
        reader.readAsDataURL(profilePicture[0]);
        var base64;
        reader.onload = function () {
            base64 = reader.result;
            debugger;
            if (base64 != "" || base64 != 0 && data.Email != "" ) {
                let applicationViewModel = JSON.stringify(data);
                $.ajax({
                    type: 'Post',
                    dataType: 'Json',
                    url: '/Account/UserRegisteration',
                    data: {
                        userDetails: applicationViewModel,
                        base64: base64
                    },
                    success: function (result) {
                        debugger;
                        if (!result.isError) {
                            var url = '/Account/Login'
                            successAlertWithRedirect(result.msg, url)
                        }
                        else {
                            errorAlert(result.msg)
                        }
                    },
                    error: function (ex) {
                        errorAlert("Error occured try again");
                    }
                })
            }
            else {
                errorAlert("Please Enter Details");
            }
        }
    }
}


function completeRegistration() {
    debugger;
    var data = {}
    data.NextOfKin = $('#nok').val();
    data.NextOfKinPhoneNumber = $('#nokno').val();
    data.HomeAddress = $('#address').val();
    data.About = $('#about').val();
    data.DepartmentId = $('#departmentId').val();
    data.CountryId = $('#countryId').val();
    data.StateId = $('#stateId').val();
    let user = JSON.stringify(data);
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/User/CompleteRegistration',
        data: {
            user: user,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/User/UserProfile?userId=' + result.id;
                successAlertWithRedirect(result.msg, url);
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function completeAdminForm() {
    debugger;
    var data = {}
    data.NextOfKin = $('#nok').val();
    data.NextOfKinPhoneNumber = $('#nokno').val();
    data.HomeAddress = $('#address').val();
    data.About = $('#about').val();
    data.DepartmentId = $('#departmentId').val();
    data.CountryId = $('#countryId').val();
    data.StateId = $('#stateId').val();
    let user = JSON.stringify(data);
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/Admin/CompleteAdminForm',
        data: {
            user: user,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/AdminProfile?userId=' + result.id;
                successAlertWithRedirect(result.msg, url);
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function login() {
    debugger;
    var data = {};
    data.Email = $('#email').val();
    data.Password = $('#password').val();
    let applicationViewModel = JSON.stringify(data);
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/Account/Login',
        data: {
            userDetails: applicationViewModel,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                location.href = result.dashboard
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function addHostel() {
    debugger;
    var hostelName = $('#hostelName').val();
    $.ajax({
        type: 'post',
        dataType: 'Json',
        url: '/Admin/AddHostel',
        data: {
            hostelName: hostelName,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/Hostels'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function hostelToBeEdited(id) {
    debugger;
    $.ajax({
        type: 'Get',
        dataType: 'Json',
        url: '/Admin/EditHostel',
        data: {
            id:id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                $('#hostelId').val(result.id);
                $('#editHostelName').val(result.name);
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function editHostel() {
    debugger;
    var data = {};
    data.Id = $('#hostelId').val();
    data.Name = $('#editHostelName').val();
    let hostelDetail = JSON.stringify(data);
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/Admin/EditHostel',
        data: {
            hostelDetails: hostelDetail,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/Hostels'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function deleteHostel() {
    var id = $('#deleteHostelId').val();
    debugger;
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/Admin/DeleteHostel',
        data: {
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/Hostels'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function hostelToBeDeleted(id) {
    debugger;
    $('#deleteHostelId').val(id);
}

function addRoom() {
    debugger;
    var data = {}
    data.HostelId = $('#hostelId').val();
    data.Name = $('#roomName').val();
    let roomDetails = JSON.stringify(data);
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/Admin/AddRoom',
        data: {
            roomViewModel: roomDetails
        },
         success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/Rooms'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function allocateHostel() {
    debugger;
    var data = {}
    data.HostelId = $('#hostelId').val();
    data.RoomId = $('#roomId').val();
    data.Id = $('#userId').val();
    let hostelDetails = JSON.stringify(data);
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/Admin/AllocateHostels',
        data: {
            hostelDetails : hostelDetails
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/HostelSettings'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function getUserId(id) {
    debugger;
    $('#userId').val(id);
}

function roomToBeEdited(id) {
    debugger;
    $.ajax({
        type: 'Get',
        dataType: 'Json',
        url: '/Admin/EditRoom',
        data: {
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                $('#roomId').val(result.id);
                $('#editRoomName').val(result.name);
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function editRoom() {
    debugger;
    var data = {};
    data.Id = $('#roomId').val();
    data.Name = $('#editRoomName').val();
    let roomDetail = JSON.stringify(data);
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/Admin/EditRoom',
        data: {
            roomDetails: roomDetail,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/Rooms'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function deleteRoom() {
    var id = $('#deleteRoomId').val();
    debugger;
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/Admin/DeleteRoom',
        data: {
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/Rooms'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function roomToBeDeleted(id) {
    debugger;
    $('#deleteRoomId').val(id);
}


function vacateUser(id) {
    debugger;

    $.ajax({
        type: 'post',
        dataType: 'Json',
        url: '/Admin/VacateHostel',
        data: {
            id: id,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/HostelSettings'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function userToBeReasigned(id) {
    debugger;
    $.ajax({
        type: 'Get',
        dataType: 'Json',
        url: '/Admin/GetUserToBeReasignedHostel',
        data: {
            userId: id
        },
        success: function (result) {
            debugger;
            $("#reassignedHostel").empty();
            $("#reassignRoom").empty();
            $.each(result.allHostels, function (i, getdropdown) {
                debugger;
                if (result.currentUser.hostel.id == getdropdown.id) {
                    $("#reassignedHostel").append('<option selected value="' + getdropdown.id + '">' + getdropdown.name + '</option>');
                }
                $("#reassignedHostel").append('<option value="' + getdropdown.id + '">' + getdropdown.name + '</option>');
            });
            $.each(result.allRooms, function (i, getdropdown) {
                debugger;
                if (result.currentUser.room.id == getdropdown.id) {
                    $("#reassignRoom").append('<option selected value="' + getdropdown.id + '">' + getdropdown.name + '</option>');
                }
                $("#reassignRoom").append('<option value="' + getdropdown.id + '">' + getdropdown.name + '</option>');
            });
            $('#reassignedUserId').val(result.currentUser.id);
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    });
}

function reasignUser() {
    debugger;
    var data = {}
    data.HostelId = $('#reassignedHostel').val();
    data.RoomId = $('#reassignRoom').val();
    data.Id = $('#reassignedUserId').val();
    let userDetails = JSON.stringify(data);
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/Admin/ReasignUser',
        data: {
            userDetails : userDetails
        },
        success: function (result) {
            debugger;
            if (!result.isError) {

                var url = '/Admin/HostelSettings'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function phoneMask() {
    var num = $(this).val().replace(/\D/g, '');
    $(this).val('+' + num.substring(0, 3) + '-' + num.substring(3, 7) + '-' + num.substring(7, 11) + '-' + num.substring(11, 14));
}
$('[type="tel"]').keyup(phoneMask);

function deactivateUser(id) {
    debugger;
    
    $.ajax({
        type: 'post',
        dataType: 'Json',
        url: '/Admin/DeactivateUser',
        data: {
            id: id,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/AllUser'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function reactivateUser(id) {
    debugger;

    $.ajax({
        type: 'post',
        dataType: 'Json',
        url: '/Admin/ReactivateUser',
        data: {
            id: id,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/AllUser'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function userProfileToBeEdited(id) {
    debugger;
    $.ajax({
        type: 'Get',
        dataType: 'Json',
        url: '/User/EditUserProfile',
        data: {
            userId: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                $('#firstName').val(result.currentUser.firstName);
                $('#lastName').val(result.currentUser.lastName);
                if (result.hostel != null) {
                    $('#hostelName').val(result.currentUser.hostel.name);
                    $('#roomName').val(result.currentUser.room.name);
                }
                $('#about').val(result.currentUser.about);
                $('#phoneNumber').val(result.currentUser.phoneNumber);
                $('#email').val(result.currentUser.email);
                $("#countryId").empty();
                $("#stateId").empty();
                $.each(result.allCountries, function (i, getdropdown) {
                    debugger;
                    if (result.currentUser.country.id == getdropdown.id) {
                        $("#countryId").append('<option selected value="' + getdropdown.id + '">' + getdropdown.name + '</option>');
                    }
                    $("#countryId").append('<option value="' + getdropdown.id + '">' + getdropdown.name + '</option>');
                });
                $.each(result.allState, function (i, getdropdown) {
                    debugger;
                    if (result.currentUser.state.id == getdropdown.id) {
                        $("#stateId").append('<option selected value="' + getdropdown.id + '">' + getdropdown.name + '</option>');
                    }
                    $("#stateId").append('<option value="' + getdropdown.id + '">' + getdropdown.name + '</option>');
                });
               
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}
function adminProfileToBeEdited(id) {
    debugger;
    $.ajax({
        type: 'Get',
        dataType: 'Json',
        url: '/Admin/EditAdminProfile',
        data: {
            userId: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                $('#firstName').val(result.firstName);
                $('#lastName').val(result.lastName);
                $('#phoneNumber').val(result.phoneNumber);
                $('#about').val(result.about);
                $('#email').val(result.email);
                $('#countryName').val(result.country.name);
                $('#stateName').val(result.state.name);

            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function editUserProfile(id) {
    debugger;
    var data = {}
    data.FirstName = $('#firstName').val();
    data.LastName = $('#lastName').val();
    data.PhoneNumber = $('#phoneNumber').val();
    data.About = $('#about').val();
    data.Email = $('#email').val();
    data.CountryId = $('#countryId').val();
    data.stateId = $('#stateId').val();
    let userProfileDetails = JSON.stringify(data);
    $.ajax({
        type: 'post',
        dataType: 'Json',
        url: '/User/EditUserProfile',
        data: {
            userProfileDetails: userProfileDetails,
            id:id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/user/UserProfile?userId=' + result.userId;
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}
function editAdminProfile(id) {
    debugger;
    var data = {}
    data.FirstName = $('#firstName').val();
    data.LastName = $('#lastName').val();
    data.PhoneNumber = $('#phoneNumber').val();
    data.About = $('#about').val();
    data.Email = $('#email').val();
    let userProfileDetails = JSON.stringify(data);
    $.ajax({
        type: 'post',
        dataType: 'Json',
        url: '/Admin/EditAdminProfile',
        data: {
            userProfileDetails: userProfileDetails,
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/admin/AdminProfile?userId=' + result.userId;
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function uploadPicture(id) {
    debugger;
    var editProfilePicture = document.getElementById("profilePicture").files;
    if (editProfilePicture[0] != null) {
        const reader = new FileReader();
        reader.readAsDataURL(editProfilePicture[0]);
        var base64;
        reader.onload = function () {
            base64 = reader.result;
            debugger;
            if (base64 != "" || base64 != 0 && id != "") {
                $.ajax({
                    type: 'Post',
                    dataType: 'Json',
                    url: '/User/EditUserProfilePicture',
                    data: {
                        id: id,
                        base64: base64,
                    },
                   
                    success: function (result) {
                        debugger;
                        if (!result.isError) {
                            var url = '/User/UserProfile?userId=' + result.userId;
                            successAlertWithRedirect(result.msg, url)
                        }
                        else {
                            errorAlert(result.msg)
                        }
                    },
                    error: function (ex) {
                        errorAlert("Error occured try again");
                    }
                })
            }
            else {
                errorAlert("Please Enter Details");
            }
        }
    }
}

function uploadPicture(id) {
    debugger;
    var editProfilePicture = document.getElementById("profilePicture").files;
    if (editProfilePicture[0] != null) {
        const reader = new FileReader();
        reader.readAsDataURL(editProfilePicture[0]);
        var base64;
        reader.onload = function () {
            base64 = reader.result;
            debugger;
            if (base64 != "" || base64 != 0 && id != "") {
                $.ajax({
                    type: 'Post',
                    dataType: 'Json',
                    url: '/Admin/EditAdminProfilePicture',
                    data: {
                        id: id,
                        base64: base64,
                    },

                    success: function (result) {
                        debugger;
                        if (!result.isError) {
                            var url = '/Admin/AdminProfile?userId=' + result.userId;
                            successAlertWithRedirect(result.msg, url)
                        }
                        else {
                            errorAlert(result.msg)
                        }
                    },
                    error: function (ex) {
                        errorAlert("Error occured try again");
                    }
                })
            }
            else {
                errorAlert("Please Enter Details");
            }
        }
    }
}

function changePassword(id) {
    debugger;
    var data = {}
    data.Password = $('#currentPassword').val();
    data.NewPassword = $('#newPassword').val();
    data.ConfirmPassword = $('#renewPassword').val();
    let userProfileDetail = JSON.stringify(data);
    $.ajax({
        type: 'post',
        dataType: 'Json',
        url: '/User/ChangeUsersPassword',
        data: {
            userProfileDetails: userProfileDetail,
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/user/UserProfile?userId=' + result.userId;
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function changePassword(id) {
    debugger;
    var data = {}
    data.Password = $('#currentPassword').val();
    data.NewPassword = $('#newPassword').val();
    data.ConfirmPassword = $('#renewPassword').val();
    let userProfileDetail = JSON.stringify(data);
    $.ajax({
        type: 'post',
        dataType: 'Json',
        url: '/Admin/ChangeAdminsPassword',
        data: {
            userProfileDetails: userProfileDetail,
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/admin/AdminProfile?userId=' + result.userId;
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}


function adminRegisteration() {
    debugger;
    var data = {};
    var profilePicture = document.getElementById("profilePicture").files;
    data.FirstName = $('#firstName').val();
    data.LastName = $('#lastName').val();
    data.GenderId = $('#genderId').val();
    data.Email = $('#email').val();
    data.PhoneNumber = $('#phoneNumber').val();
    data.Password = $('#password').val();
    data.ConfirmPassword = $('#confirmPassword').val();
    if (profilePicture[0] != null) {
        const reader = new FileReader();
        reader.readAsDataURL(profilePicture[0]);
        var base64;
        reader.onload = function () {
            base64 = reader.result;
            debugger;
            if (base64 != "" || base64 != 0 && data.Email != "") {
                let applicationViewModel = JSON.stringify(data);
                $.ajax({
                    type: 'Post',
                    dataType: 'Json',
                    url: '/Account/AdminRegisteration',
                    data: {
                        userDetails: applicationViewModel,
                        base64: base64
                    },
                    success: function (result) {
                        debugger;
                        if (!result.isError) {
                            var url = '/Account/Login'
                            successAlertWithRedirect(result.msg, url)
                        }
                        else {
                            errorAlert(result.msg)
                        }
                    },
                    error: function (ex) {
                        errorAlert("Error occured try again");
                    }
                })
            }
            else {
                errorAlert("Please Enter Details");
            }
        }
    }
}

function updateUser() {
    debugger;
    var data = {}
    data.Email = $('#userEmail').val();
    data.NewPassword = $('#newPassword').val();
    data.ConfirmPassword = $('#confirmPassword').val();
    let applicationUserViewModel = JSON.stringify(data);
    $.ajax({
        type: 'post',
        dataType: 'Json',
        url: '/Account/ChangeUserPassword',
        data: {
            applicationUserViewModel: applicationUserViewModel,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {

                var url = '/Account/login'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}



function verifyEmailToForgetPassword() {
    debugger;
    var email = $('#emailToVerify').val();
    $.ajax({
        type: 'POST',
        dataType: 'Json',
        url: '/Account/VerifyUserToChangingPswd',
        data:
        {
            userName : email
        },
        success: function (result)
        {
            debugger;
            if (!result.isError)
            {
                $("#passwordField").show();
                $('#userEmail').val(result.data.email);
                $("#email").hide();
            }
            else {
                errorAlert(result.msg)
            }

        },
        error: function (ex) {
            errorAlert('Failed to retrieve Email' + ex)
        }
    })
}


function ApplicantDocumentation() {
    debugger;
    var file = {};
    file.FirstGuarantor = document.getElementById("FirstGuarantor").files;
    file.SecondGuarantor = document.getElementById("SecondGuarantor").files;
    file.NepaBill = document.getElementById("NepaBill").files;
    file.Contractforms = document.getElementById("Contractforms").files;
    var BVN = $('#BVN').val();

    if (file.FirstGuarantor[0] != null) {
        const reader = new FileReader();
        reader.readAsDataURL(file.FirstGuarantor[0]);
        var base64FirstGuarantor;
        reader.onload = function () {
            base64FirstGuarantor = reader.result;
            debugger;

            if (file.SecondGuarantor[0] != null) {
                const reader = new FileReader();
                reader.readAsDataURL(file.SecondGuarantor[0]);
                var base64SecondGuarantor;
                reader.onload = function () {
                    base64SecondGuarantor = reader.result;
                    debugger;
                    if (file.NepaBill[0] != null) {
                        const reader = new FileReader();
                        reader.readAsDataURL(file.NepaBill[0]);
                        var base64NepaBill;
                        reader.onload = function () {
                            base64NepaBill = reader.result;
                            debugger;
                            if (file.Contractforms[0] != null) {
                                const reader = new FileReader();
                                reader.readAsDataURL(file.Contractforms[0]);
                                var base64Contractforms;
                                reader.onload = function () {
                                    base64Contractforms = reader.result;
                                    var allDocument = {};
                                    allDocument.BVN = BVN;
                                    allDocument.FirstGuarantor = base64FirstGuarantor;
                                    allDocument.SecondGuarantor = base64SecondGuarantor;
                                    allDocument.NepaBill = base64NepaBill;
                                    allDocument.SignedContract = base64Contractforms;
                                    debugger;
                                    if (BVN != "" || BVN != 0) {
                                        let rawData = JSON.stringify(allDocument);
                                        $.ajax({
                                            type: 'Post',
                                            dataType: 'Json',
                                            url: '/Applicant/SubmitApplicantDocument',
                                            data:
                                            {
                                                applicantDetailedDocuments: rawData,
                                            },
                                            success: function (result) {
                                                debugger;
                                                if (!result.isError) {
                                                    successAlert(result.msg)
                                                    window.location.reload;
                                                }
                                                else {
                                                    errorAlert(result.msg)
                                                }
                                            },
                                            error: function (ex) {
                                                errorAlert("Error occured try again");
                                            }
                                        })
                                    }
                                    else {
                                        errorAlert("Incorrect Details");
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
