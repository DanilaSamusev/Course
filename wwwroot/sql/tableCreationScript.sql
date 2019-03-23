create table if not exists `student_rating_base`.users
(
  id       bigint auto_increment primary key,
  login    varchar(256) null,
  password int          null,
  role     varchar(256) null,
  constraint users_id_uindex
  unique (id)  
);

create table if not exists `student_rating_base`.students
(
  id           bigint auto_increment primary key,
  group_number int          null,
  name         varchar(256) null,
  surname      varchar(256) null,
  patronymic   varchar(256) null,
  constraint students_id_uindex
  unique (id)
);

create table if not exists `student_rating_base`.exams
(
  id                  bigint auto_increment primary key,
  student_id          bigint       null,  
  philosophy          int          null,
  psychology          int          null,
  mathematics         int          null,
  physics             int          null,
  programming         int          null,
  constraint exams_id_uindex
  unique (id),
  constraint exams_student_id_uindex
  unique (student_id),
  constraint exams_students_id_fk
  foreign key (student_id) references students (id)
    on update cascade
    on delete cascade
);

create table if not exists `student_rating_base`.scores
(
  id                  bigint auto_increment primary key,
  student_id          bigint       null,
  history             varchar(256) null,
  `political _science` varchar(256) null,
  PE                  varchar(256) null,
  foreign_language    varchar(256) null,
  chemistry           varchar(256) null,
  constraint scores_id_uindex
  unique (id),
  constraint scores_student_id_uindex
  unique (student_id),
  constraint scores_students_id_fk
  foreign key (student_id) references students (id)
    on update cascade
    on delete cascade
);
