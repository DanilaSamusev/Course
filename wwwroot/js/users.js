function modifyUser(state, id){
    
    document.getElementById('modifyWindow').style.display = state;
    document.getElementById('id').value = id;
}

function addUser(state){
    document.getElementById('addWindow').style.display = state;
}