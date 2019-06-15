package de.htw.cg.pingpong.controller;

import de.htw.cg.pingpong.model.Position;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.Payload;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.stereotype.Controller;

@Controller
public class PositionController {

    @MessageMapping("/position")
    @SendTo("/game/position")
    public Position sendPosition(@Payload Position position) {
        return position;
    }

}
