function modifyStudent(state, id){

    document.getElementById('modifyWindow').style.display = state;
    document.getElementById('modifyId').value = id;
}

function addStudent(state){
    document.getElementById('addWindow').style.display = state;
}

function deleteStudent(state, id){

    document.getElementById('deleteWindow').style.display = state;
    document.getElementById('deleteId').value = id;
}