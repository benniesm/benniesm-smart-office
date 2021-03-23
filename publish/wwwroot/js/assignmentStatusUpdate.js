
let apiAddress1 = '';
const appOrigin1 = window.location.origin;
apiAddress1 = appOrigin1 + '/api/StatusUpdate/';// console.log('me');

const formatDate = (d) => {
    let dformat = [d.getFullYear(),
        d.getMonth()+1,
        d.getDate()].join('-')+' '+
       [d.getHours(),
        d.getMinutes(),
        d.getSeconds()].join(':');
    return dformat;
}

const sendPutRequest = (id, data) => {
    let xsrfToken = data.__RequestVerificationToken;
    delete data.__RequestVerificationToken;
    data.Started = data.Started === '' ? null : data.Started;
    data.Executed = data.Executed === '' ? null : data.Executed;
    data.Completed = data.Completed === '' ? null : data.Completed;

    //console.log(data);
    //console.log(id);
    //console.log(xsrfToken);
    //console.log(apiAddress+id);

    fetch(apiAddress1 + id, {
        method: 'PUT',
        body: JSON.stringify(data),
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': xsrfToken
        }
    })
    .then(response => response.json()
        .then(data => ({
            status: response.status,
            body: data
        }))
    )
    .then(response => {
        //console.log(response);
        if (response.status === 200) {
            window.alert('Status successfully updated!')
            location.reload();
        } else {
            window.alert('Connection error. Please try again.');
        }
    })
    .catch(error => {
        window.alert('Connection error. Please try again.');
    });
}

const startStatusPend = () => {
    setTimeout(() => {
        if (!confirm('Are you sure you want to update the Task Status?')) {
            document.getElementById('status-changer').value = 'default';
            return;
        }
    
        document.getElementById('execute').style.display = 'none';
        document.getElementById('complete').style.display = 'none';
        document.getElementById('status-change-fields').style.display = 'none';
        document.getElementById('status').value = 'pending';
        const formFields = Array.from(document.querySelectorAll('#status-change-form input'));
        const taskId = document.getElementById('task-id').value;
        let formData = {};
    
        formFields.forEach(field => {
            formData[field.id] = field.value;
        });
    
        formData.TimeIn = new Date(formData.TimeIn).toISOString();
        let dateToday = new Date();
        formData.Started = dateToday.toISOString();
        formData.Deadline = new Date(formData.Deadline).toISOString();
        formData.ExecutedNotes = '';
        formData.CompletionNotes = '';
        //console.log(formData);
    
        sendPutRequest(taskId, formData);    
    }, 1000);   
}

const startStatusExecute = () => {
    document.getElementById('complete').style.display = 'none';
    document.getElementById('execute').style.display = 'inline-block';
    document.getElementById('status-change-fields').style.display = 'block';
    document.getElementById('status-change-form')
        .addEventListener('submit', finishStatusExecute);
}

const finishStatusExecute = () => {
    document.getElementById('status').value = 'executed';
    
    const formFields = Array.from(document.querySelectorAll('#status-change-form input'));
    const taskId = document.getElementById('task-id').value;
    let formData = {};
    formFields.forEach(field => {
        formData[field.id] = field.value;
    });
    formData.ExecutedNotes = document.getElementById('ExecutedNotes').value;
    formData.CompletionNotes = '';

    formData.TimeIn = new Date(formData.TimeIn).toISOString();
    formData.Deadline = new Date(formData.Deadline).toISOString();
    formData.Started = formData.Started === '' ? '' : new Date(formData.Started).toISOString();
    let dateToday = new Date();
    formData.Executed = dateToday.toISOString();
    //console.log(formData);

    sendPutRequest(taskId, formData);
}

const startStatusComplete = () => {
    document.getElementById('execute').style.display = 'none';
    document.getElementById('complete').style.display = 'inline-block';
    document.getElementById('status-change-fields').style.display = 'block';
    document.getElementById('status-change-form')
        .addEventListener('submit', finishStatusComplete);
}

const finishStatusComplete = () => {
    //return console.log('finishing');
    document.getElementById('status').value = 'completed';
    const formFields = Array.from(document.querySelectorAll('#status-change-form input'));
    const taskId = document.getElementById('task-id').value;
    let formData = {};

    formFields.forEach(field => {
        formData[field.id] = field.value;
    });
    formData.ExecutedNotes = document.getElementById('ExecutedNotes').value;
    formData.CompletionNotes = document.getElementById('CompletionNotes').value;

    formData.TimeIn = new Date(formData.TimeIn).toISOString();
    formData.Deadline = new Date(formData.Deadline).toISOString();
    formData.Started = formData.Started === '' ? '' : new Date(formData.Started).toISOString();
    formData.Executed = formData.Executed === '' ? '' : new Date(formData.Executed).toISOString();
    let dateToday = new Date();
    formData.Completed = dateToday.toISOString();
    //console.log(formData);

    sendPutRequest(taskId, formData);
}

const endStatusUpdate = () => {
    document.getElementById('execute').style.display = 'none';
    document.getElementById('complete').style.display = 'none';
    document.getElementById('status-change-fields').style.display = 'none';
    document.getElementById('status-changer').value = 'default';
}

const startStatusChange = (e) => {
    let optionTaken = e.target.value;
    
    switch (optionTaken) {
        case 'pending':
            startStatusPend();
            break;
        case 'executed':
            startStatusExecute();
            break;
        case 'completed':
            startStatusComplete();
            break;
        default:
            endStatusUpdate();
            break;
    }
}

const init = () => {    
    const taskStatus = document.getElementById('task-status').value;

    if (taskStatus !== 'completed') {
        document.getElementById('status-changer').style.display = 'inline-block';
    }

    document.getElementById('status-changer')
    .addEventListener('change', startStatusChange);
}

init();
