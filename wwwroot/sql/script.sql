create table students
(
  id           bigint auto_increment,
  group_number int          null,
  name         varchar(256) null,
  surname      varchar(256) null,
  patronymic   varchar(256) null,
  constraint students_id_uindex
  unique (id)
);

alter table students
  add primary key (id);

create table rating
(
  student_id          bigint auto_increment,
  history             varchar(256) null,
  `political _cience` varchar(256) null,
  PE                  varchar(256) null,
  foreign_language    varchar(256) null,
  chemistry           varchar(256) null,
  philosophy          int          null,
  psychology          int          null,
  mathematics         int          null,
  physics             int          null,
  programming         int          null,
  constraint rating_student_id_uindex
  unique (student_id),
  constraint rating_students_id_fk
  foreign key (student_id) references students (id)
    on update cascade
    on delete cascade
);

alter table rating
  add primary key (student_id);

create table users
(
  id       bigint auto_increment,
  login    varchar(256) null,
  password int          null,
  role     varchar(256) null,
  constraint users_id_uindex
  unique (id)
);

alter table users
  add primary key (id);


