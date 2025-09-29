CREATE TABLE Blogposts (
    BlogPostId CHAR(36) NOT NULL PRIMARY KEY,
    AuthorId CHAR(36) NULL,
    Title VARCHAR(255) NULL,
    Description TEXT NOT NULL DEFAULT '',
    PublishedAt DATETIME NULL,
    Likes BIGINT NOT NULL DEFAULT 0,
    DisLikes BIGINT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
);


CREATE TABLE Comments (
    CommentId CHAR(36) NOT NULL PRIMARY KEY,
    BlogPostId CHAR(36) NOT NULL,
    AuthorId CHAR(36) NULL,
    ParentId CHAR(36) NULL,
    Content TEXT NOT NULL DEFAULT '',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
   
    CONSTRAINT FK_Comment_Blogpost FOREIGN KEY (BlogPostId)
        REFERENCES Blogpost(BlogPostId)
        ON DELETE CASCADE,
        
    CONSTRAINT FK_Comment_Parent FOREIGN KEY (ParentId)
        REFERENCES Comment(CommentId)
        ON DELETE CASCADE
);


CREATE INDEX IDX_Comment_BlogPostId ON Comment(BlogPostId);
CREATE INDEX IDX_Comment_ParentId ON Comment(ParentId);
