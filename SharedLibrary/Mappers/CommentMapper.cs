using SharedLibrary.Model;
using SharedLibrary.DTOs;
namespace SharedLibrary.Mappers
{
    public static class CommentMapper
    {
        public static CommentDTO EntityToDTO(Comment comment)
        {
            return new CommentDTO
            {
                CommentId=comment.CommentId,
                CommentText=comment.CommentText,
                CommentTimestamp=comment.CommentTimestamp
            };
        }

        public static Comment DtoToEntity(CommentDTO commentDTO)
        {
            return new Comment
            {
                CommentId=commentDTO.CommentId,
                CommentText=commentDTO.CommentText,
                CommentTimestamp=commentDTO.CommentTimestamp
            };
        }
    }
}
