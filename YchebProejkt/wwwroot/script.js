let activeCatalogue = "инструкции";

const uploadEndpoints = {
    "инструкции": "/api/instruction/upload",
    "регистры": "/api/registry",
    "управления": "/api/managment"
};

function setCatalogue(name) {
    activeCatalogue = name;
    document.getElementById("activeCatalogue").innerText = name;
    loadCurrentCatalogue();
}

function loadCurrentCatalogue() {
    const instructionFields = document.getElementById("instructionFields");
    const registryFields = document.getElementById("registryFields");

    if (instructionFields) instructionFields.style.display = "none";
    if (registryFields) registryFields.style.display = "none";

    switch (activeCatalogue) {
        case "инструкции":
            if (instructionFields) instructionFields.style.display = "block";
            loadInstructions();
            break;

        case "регистры":
            if (registryFields) registryFields.style.display = "block";
            loadRegistries();
            break;

        case "управления":
            loadManagements();
            break;
    }
}

function loadCurrentList() {
    switch (activeCatalogue) {
        case "инструкции":
            loadInstructions();
            break;
        case "регистры":
            loadRegistries();
            break;
        case "управления":
            loadManagements();
            break;
    }
}

async function loadRegistryDropdown() {
    const res = await fetch("/api/Registry");
    const data = await res.json();

    const filter = document.getElementById("registryFilter");
    const select = document.getElementById("registrySelect");
    console.log(data);
    data.forEach(r => {
        const option1 = document.createElement("option");
        option1.value = r.id;
        option1.textContent = r.name;

        const option2 = document.createElement("option");
        option2.value = r.id;
        option2.textContent = r.name;

        filter.appendChild(option1);
        select.appendChild(option2);
    });
}

async function loadManagementDropdown() {
    const res = await fetch("/api/managment");
    const data = await res.json();
    const select = document.getElementById("managementSelect");

    data.forEach(m => {
        const option = document.createElement("option");
        option.value = m.id;
        option.textContent = m.name;
        select.appendChild(option);
    });
}

//загрузка инструкций/реестров/управлений
async function upload() {
    const endpoint = uploadEndpoints[activeCatalogue];

    const formData = new FormData();
    //formData.append("catalogue", activeCatalogue);

    if (activeCatalogue === "инструкции") {
        const fileInput = document.getElementById("formFile");
        if (!fileInput.files.length) {
            alert("Выберите файл");
            return;
        }

        formData.append("file", fileInput.files[0]);
        formData.append("registryId", document.getElementById("registrySelect").value);
        formData.append("title", document.getElementById("title").value);
    }

    if (activeCatalogue === "регистры") {
        formData.append("Name", document.getElementById("title").value);
        formData.append("ManagementId", document.getElementById("managementSelect").value);
    }

    if (activeCatalogue === "управления") {
        formData.append("Name", document.getElementById("title").value);
    }

    const response = await fetch(endpoint, {
        method: "POST",
        body: formData
    });

    if (response.ok) {
        alert("Загружено!");
        loadCurrentList();
    } else {
        alert("Ошибка загрузки!");
    }
}

function renderList(items, type) {
    const container = document.getElementById("instructions");
    container.innerHTML = "";

    items.forEach(item => {
        const col = document.createElement("div");
        col.className = "col-md-6";

        col.innerHTML = getCardTemplate(item, type);

        container.appendChild(col);
    });
}

function getCardTemplate(item, type) {
    if (type === "инструкции") {
        return `
            <div class="card bg-dark border-light text-white h-100">
                <div class="card-body">
                    <h5 class="card-title">${item.title}</h5>

                    <p class="card-text">
                        <small>ID: ${item.id}</small><br>
                        <small>Регистр: ${item.registryId ?? "-"}</small>
                    </p>

                    <button class="btn btn-outline-light btn-sm me-2"
                        onclick="downloadInstruction(${item.id})">
                        Скачать
                    </button>

                    <button class="btn btn-outline-danger btn-sm"
                        onclick="deleteInstruction(${item.id})">
                        Удалить
                    </button>
                </div>
            </div>
        `;
    }
    if (type === "регистры") {
        return `
            <div class="card bg-dark border-light text-white h-100">
                <div class="card-body">
                    <h5 class="card-title">${item.name}</h5>

                    <p class="card-text">
                        <small>ID: ${item.id}</small><br>
                        <small>Управление: ${item.managementId ?? "-"}</small>
                    </p>

                    <button class="btn btn-outline-danger btn-sm"
                        onclick="deleteRegistry(${item.id})">
                        Удалить
                    </button>
                </div>
            </div>
        `;
    }
    if (type === "управления") {
        return `
            <div class="card bg-dark border-light text-white h-100">
                <div class="card-body">
                    <h5 class="card-title">${item.name}</h5>

                    <p class="card-text">
                        <small>ID: ${item.id}</small>
                    </p>

                    <button class="btn btn-outline-danger btn-sm"
                        onclick="deleteManagement(${item.id})">
                        Удалить
                    </button>
                </div>
            </div>
        `;
    }

    return "";
}

//поиск инструкций
async function loadInstructions() {
    const title = document.getElementById("searchTitle")?.value || "";
    const registryId = document.getElementById("registryFilter")?.value;
    const search = document.getElementById("searchForInstructions");
    search.classList.remove("d-none");
    console.log(search);
    let url = `/api/instruction/search?`;

    if (title) url += `title=${encodeURIComponent(title)}&`;
    if (registryId) url += `registryId=${registryId}`;

    const response = await fetch(url);
    const data = await response.json();

    renderList(data, "инструкции");
}

function loadRegistries() {
    const search = document.getElementById("searchForInstructions");
    search.classList.add("d-none");
    console.log(search);
    fetch("/api/registry")
        .then(r => r.json())
        .then(data => renderList(data, "регистры"));
}

function loadManagements() {
    const search = document.getElementById("searchForInstructions");
    search.classList.add("d-none");
    console.log(search);
    fetch("/api/managment")
        .then(r => r.json())
        .then(data => renderList(data, "управления"));
}

//скачать инструкции
async function downloadInstruction(id) {
    const response = await fetch(`/api/instruction/download?instructionId=${id}`);

    if (!response.ok) {
        alert("Загрузка не удалась");
        return;
    }

    const blob = await response.blob();

    const disposition = response.headers.get("content-disposition");
    const filename = getFileNameFromHeader(disposition);

    const url = window.URL.createObjectURL(blob);

    const a = document.createElement("a");
    a.href = url;
    a.download = filename;

    document.body.appendChild(a);
    a.click();

    a.remove();
    window.URL.revokeObjectURL(url);
}

async function deleteInstruction(id) {
    const confirmDelete = confirm("Удалить инструкцию?");

    if (!confirmDelete) return;

    const response = await fetch(`/api/instruction/${id}`, {
        method: "DELETE"
    });

    if (response.ok) {
        alert("Удалено!");
        loadInstructions();
    } else {
        alert("Ошибка удаления");
    }
}

async function deleteRegistry(id) {
    const confirmDelete = confirm("Удалить Регистр? (Должен быть пустым)");

    if (!confirmDelete) return;

    const response = await fetch(`/api/registry/${id}`, {
        method: "DELETE"
    });

    if (response.ok) {
        alert("Удалено!");
        loadInstructions();
    } else {
        alert("Ошибка удаления");
    }
}

async function deleteManagement(id) {
    const confirmDelete = confirm("Удалить управление? (Должно быть пустым)");

    if (!confirmDelete) return;

    const response = await fetch(`/api/managment/${id}`, {
        method: "DELETE"
    });

    if (response.ok) {
        alert("Удалено!");
        loadInstructions();
    } else {
        alert("Ошибка удаления");
    }
}

function getFileNameFromHeader(disposition) {
    if (!disposition) return "file";

    let match = disposition.match(/filename\*\=UTF-8''(.+)/);

    if (match) {
        return decodeURIComponent(match[1]);
    }

    match = disposition.match(/filename="(.+?)"/);
    if (match) {
        return match[1];
    }

    return "file";
}

function refresh(){
    loadCurrentList()
    loadRegistryDropdown();
    loadManagementDropdown();
}

document.addEventListener("DOMContentLoaded", () => {
    loadRegistryDropdown();
    loadManagementDropdown();
    loadInstructions();
});