
//BEGIN Sweet alert
function ClientConfirm(obj, event, title, msg, type) {
    event.preventDefault();
    swal({
        title: title,
        text: msg,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Save",
        cancelButtonText: "Cancel",
        closeOnConfirm: true,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                __doPostBack('<%= Page.ClientID %>', obj.name);
                return true;
            } else {
                return false;

            }
        });
}
function ClientConfirmDel(obj, event, title, msg, type) {
    event.preventDefault();
    swal({
        title: title,
        text: msg,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Delete",
        cancelButtonText: "Cancel",
        closeOnConfirm: true,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                __doPostBack('<%= Page.ClientID %>', obj.name);
                return true;
            } else {
                return false;

            }
        });
}
function ServerAlert(title, msg, type) {
    swal({
        title: title,
        text: msg,
        type: type,
    });
}

//END Sweet alert
//BEGIN DIGIT ONLY          
function DigitOnly(txt, event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode == 46) {
        if (txt.value.indexOf(".") < 0)
            return true;
        else
            return false;
    }
    if (charCode > 31 && (charCode < 45 || charCode > 57))
        return false;

    return true;
}
//END DIGIT ONLY

//BEGIN ENGLIST ONLY  
function EnglistOnly(event) {
    $(this).keypress(function (event) {
        var ew = event.which;
        if (ew == 32)
            return true;
        if (48 <= ew && ew <= 57 && ew == 45)
            return true;
        if (65 <= ew && ew <= 90)
            return true;
        if (97 <= ew && ew <= 122)
            return true;
        return false;
    });
}
            //END ENGLIST ONLY 