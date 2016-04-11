function changeLogoffOnClick() {    
    $("#logoutlink").click(function (e) {
        $("#logoutForm").submit();        
        return false;
    });    
}


$(document).ready(changeLogoffOnClick);
